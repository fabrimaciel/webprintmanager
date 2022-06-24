namespace WebPrintManager
{
    public class PrintManager : IDisposable
    {
        private readonly PrintManagerConnection connection;
        private readonly IDictionary<Messages.MessageType, IReceiveMessageCallback> callbacks = new Dictionary<Messages.MessageType, IReceiveMessageCallback>();

        public event EventHandler? ConnectionChanged;

        public PrintManager()
        {
            this.connection = new PrintManagerConnection();
            this.connection.ReceivedMessage += this.ConnectionReceivedMessage;
            this.connection.WsClosed += this.ConnectionWsClosed;
            this.ConfigureCallback<Messages.ConnectionMessage>(Messages.MessageType.Connection, this.OnConnection);
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

        public Task Start(string host, int port)
        {
            return this.connection.Connect(host, port);
        }

        public Task<IEnumerable<string>> GetPrinters(CancellationToken cancellationToken) =>
            this.Execute<IEnumerable<string>>(Messages.MessageType.PrintersList, null, cancellationToken) !;

        public async Task RawPrint(string printerName, byte[] data, CancellationToken cancellationToken)
        {
            var encoding = System.Text.Encoding.UTF8;
            var body = new byte[encoding.GetByteCount(printerName) + 1 + data.Length];

            encoding.GetBytes(printerName, 0, printerName.Length, body, 0);
            body[body.Length - data.Length - 1] = (byte)',';
            Buffer.BlockCopy(data, 0, body, body.Length - data.Length, data.Length);

            var result = (await this.ExecuteBinary<Messages.RawPrintResult>(Messages.MessageType.RawPrint, body, cancellationToken)) !;

            if (!result.Success)
            {
                throw new InvalidOperationException(result.Error);
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
