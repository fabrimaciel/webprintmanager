namespace WebPrintManager.Agent.Services
{
    internal class PrintersService : IReceivedMessageCallback
    {
        private readonly ManagerSession session;

        public PrintersService(ManagerSession session)
        {
            this.session = session;
        }

        public void Execute(string tag, byte[] buffer, int offset, int size)
        {
            var printers = new List<string>();

#pragma warning disable CA1416 // Validate platform compatibility
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                printers.Add(printer);
            }
#pragma warning restore CA1416 // Validate platform compatibility

            this.session.SendJson(printers, "message", tag, false);
        }
    }
}
