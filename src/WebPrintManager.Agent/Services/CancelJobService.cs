namespace WebPrintManager.Agent.Services
{
    internal class CancelJobService : IReceivedMessageCallback
    {
        private readonly ManagerSession session;
        private readonly Printing.PrintMonitor printMonitor;
        private readonly Printing.ClientPrinterFactory clientPrinterFactory;

        public CancelJobService(
            ManagerSession session,
            Printing.PrintMonitor printMonitor,
            Printing.ClientPrinterFactory clientPrinterFactory)
        {
            this.session = session;
            this.printMonitor = printMonitor;
            this.clientPrinterFactory = clientPrinterFactory;
        }

        public void Execute(string tag, byte[] buffer, int offset, int size)
        {
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
                            queue.CancelJob((uint)message.JobId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.session.SendJson(new Messages.GenericResult(false, ex.Message), "message", tag, false);
                    return;
                }

                this.session.SendJson(new Messages.GenericResult(true, null), "message", tag, false);
            }
        }
    }
}
