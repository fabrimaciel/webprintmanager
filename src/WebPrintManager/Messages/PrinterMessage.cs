namespace WebPrintManager.Messages
{
    internal class PrinterMessage
    {
        public PrinterMessage(IClientPrinter printer)
        {
            this.PrinterType = printer.GetType().Name;
            this.PrinterData = System.Text.Json.JsonSerializer.Serialize(printer, printer.GetType());
        }

        public string PrinterType { get; }

        public string PrinterData { get; }
    }
}
