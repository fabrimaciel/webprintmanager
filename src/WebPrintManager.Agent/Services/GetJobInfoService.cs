namespace WebPrintManager.Agent.Services
{
    internal class GetJobInfoService : IReceivedMessageCallback
    {
        private readonly ManagerSession session;
        private readonly Printing.PrintMonitor printMonitor;
        private readonly Printing.ClientPrinterFactory clientPrinterFactory;

        public GetJobInfoService(
            ManagerSession session,
            Printing.PrintMonitor printMonitor,
            Printing.ClientPrinterFactory clientPrinterFactory)
        {
            this.session = session;
            this.printMonitor = printMonitor;
            this.clientPrinterFactory = clientPrinterFactory;
        }

        private Messages.GetPrintJobInfoResult CreateMessage(Printing.Win32.JOB_INFO_2 info)
        {
            return new Messages.GetPrintJobInfoResult
            {
                JobId = (int)info.JobId,
                Status = (Messages.PrintJobStatus)(int)info.Status,
                PagesPrinted = (int)info.PagesPrinted,
                TotalPages = (int)info.TotalPages,
            };
        }

        public void Execute(string tag, byte[] buffer, int offset, int size)
        {
            Messages.GetPrintJobInfoResult? response = null;

            using (var inputStream = new MemoryStream(buffer, offset, size))
            {
                var message = System.Text.Json.JsonSerializer.Deserialize<Messages.PrintJobMessage>(inputStream) !;
                var printer = this.clientPrinterFactory.Create(message.PrinterType, message.PrinterData);

                try
                {
                    if (printer is Printing.ISpoolerPrinter spoolerPrinter)
                    {
                        var queue = this.printMonitor.Configure(spoolerPrinter.SpoolerName);

                        if (queue != null)
                        {
                            var info = queue.GetJob((uint)message.JobId);

                            if (info.HasValue)
                            {
                                response = this.CreateMessage(info.Value);
                            }
                        }
                    }
                }
                catch
                {
                    // ignore
                }

                this.session.SendJson(
                    response,
                    "message",
                    tag,
                    false);
            }
        }
    }
}
