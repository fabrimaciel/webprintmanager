namespace WebPrintManager.Messages
{
    internal class PrintJobMessage
    {
        public PrintJobMessage(
            IClientPrinter printer,
            int jobId)
        {
            this.PrinterType = printer.GetType().Name;
            this.PrinterData = System.Text.Json.JsonSerializer.Serialize(printer, printer.GetType());
            this.JobId = jobId;
        }

        public string PrinterType { get; }

        public string PrinterData { get; }

        public int JobId { get; }
    }
}
