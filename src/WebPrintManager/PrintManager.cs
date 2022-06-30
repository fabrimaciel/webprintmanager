namespace WebPrintManager
{
    public class PrintManager : IDisposable
    {
        private readonly PrintManagerConnection connection;
        private readonly IDictionary<Messages.MessageType, IReceiveMessageCallback> callbacks = new Dictionary<Messages.MessageType, IReceiveMessageCallback>();

        public event EventHandler? ConnectionChanged;

        internal event EventHandler<PrintJobStatusChangedEventArgs>? PrintJobStatusChanged;

        public PrintManager()
        {
            this.connection = new PrintManagerConnection();
            this.connection.ReceivedMessage += this.ConnectionReceivedMessage;
            this.connection.WsClosed += this.ConnectionWsClosed;
            this.ConfigureCallback<Messages.ConnectionMessage>(Messages.MessageType.Connection, this.OnConnection);
            this.ConfigureCallback<Messages.PrintJobStatusMessage>(Messages.MessageType.PrintJobStatus, this.OnPrintJobStatusChanged);
        }

        public bool IsConnected { get; private set; }

        private void ConfigureCallback<T>(Messages.MessageType type, Action<T> callback)
        {
            this.callbacks.Add(type, new ReceiveMessageCallback<T>(callback));
        }

        private async Task<T?> Execute<T>(Messages.MessageType messageType, object? input, CancellationToken cancellationToken)
        {
            var tag = await this.connection.SendJson(input, messageType, null, false);
            var response = await this.connection.WaitForMessage(tag, 10000, cancellationToken);

            if (string.IsNullOrEmpty(response))
            {
                return default(T);
            }

            return System.Text.Json.JsonSerializer.Deserialize<T>(response) !;
        }

        private async Task<T?> ExecuteBinary<T>(Messages.MessageType messageType, byte[] data, CancellationToken cancellationToken)
        {
            var tag = await this.connection.SendBinary(data, messageType, null, false);
            var response = await this.connection.WaitForMessage(tag, 10000, cancellationToken);

            if (string.IsNullOrEmpty(response))
            {
                return default(T);
            }

            return System.Text.Json.JsonSerializer.Deserialize<T>(response) !;
        }

        private void ConnectionWsClosed(object? sender, string e)
        {
            this.IsConnected = false;
            this.ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool ConnectionReceivedMessage(ReceivedMessage e)
        {
            if (this.callbacks.TryGetValue(e.Type, out var callback))
            {
                callback.Execute(e);
            }

            return true;
        }

        private void OnConnection(Messages.ConnectionMessage message)
        {
            this.IsConnected = message.Connection == "CONNECTED";
            this.ConnectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnPrintJobStatusChanged(Messages.PrintJobStatusMessage message)
        {
            this.PrintJobStatusChanged?.Invoke(this, new PrintJobStatusChangedEventArgs(message.JobId, message.Status));
        }

        public Task Start(string host, int port)
        {
            return this.connection.Connect(host, port);
        }

        internal Task<Messages.GetPrintJobInfoResult?> GetJobInfo(IClientPrinter printer, int jobId, CancellationToken cancellationToken)
        {
            return this.Execute<Messages.GetPrintJobInfoResult>(
                Messages.MessageType.GetPrintJobInfo,
                new Messages.PrintJobMessage(printer, jobId),
                cancellationToken);
        }

        internal async Task CancelJob(IClientPrinter printer, int jobId, CancellationToken cancellationToken)
        {
            (await this.Execute<Messages.GenericResult>(
                Messages.MessageType.CancelJob,
                new Messages.PrintJobMessage(printer, jobId),
                cancellationToken))?.ThrowOnFail();
        }

        public Task<IEnumerable<string>> GetPrinters(CancellationToken cancellationToken)
        {
            return this.Execute<IEnumerable<string>>(Messages.MessageType.PrintersList, null, cancellationToken) !;
        }

        public async Task<ClientPrintJob> RawPrint(
            IClientPrinter printer,
            PrintDocumentInfo documentInfo,
            byte[] data,
            CancellationToken cancellationToken)
        {
            if (printer is null)
            {
                throw new ArgumentNullException(nameof(printer));
            }

            if (documentInfo is null)
            {
                throw new ArgumentNullException(nameof(documentInfo));
            }

            var encoding = System.Text.Encoding.UTF8;
            var printerType = printer.GetType().Name;
            var printerData = System.Text.Json.JsonSerializer.Serialize(printer, printer.GetType());
            var documentInfoData = System.Text.Json.JsonSerializer.Serialize(documentInfo);

            Messages.RawPrintResult result;

            using (var outputStream = new MemoryStream())
            {
                var writer = new BinaryWriter(outputStream, encoding);
                writer.Write(printerType);
                writer.Write(printerData);
                writer.Write(documentInfoData);
                writer.Write(data.Length);
                writer.Write(data, 0, data.Length);
                writer.Flush();
                outputStream.Flush();

                result = (await this.ExecuteBinary<Messages.RawPrintResult>(Messages.MessageType.RawPrint, outputStream.ToArray(), cancellationToken)) !;
            }

            if (!result.Success)
            {
                throw new InvalidOperationException(result.Error);
            }

            var info = await this.GetJobInfo(printer, result.JobId, cancellationToken);

            if (info == null)
            {
                return new ClientPrintJob(
                    new Messages.GetPrintJobInfoResult
                    {
                        JobId = result.JobId,
                        Status = PrintJobStatus.Completed,
                    },
                    printer,
                    this);
            }

            return new ClientPrintJob(info, printer, this);
        }

        public async Task<PrinterInfo> GetPrinter(IClientPrinter printer, CancellationToken cancellationToken)
        {
            var result = await this.Execute<Messages.GetPrinterInfoResult>(
                Messages.MessageType.PrinterInfo,
                new Messages.PrinterMessage(printer),
                cancellationToken);

            return new PrinterInfo(result);
        }

        public async Task Purge(IClientPrinter printer, CancellationToken cancellationToken)
        {
            var result = await this.Execute<Messages.GenericResult>(
                Messages.MessageType.PrinterPurge,
                new Messages.PrinterMessage(printer),
                cancellationToken);

            if (result != null && !result.Success)
            {
                throw new InvalidOperationException(result.Message);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            this.connection.Dispose();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
