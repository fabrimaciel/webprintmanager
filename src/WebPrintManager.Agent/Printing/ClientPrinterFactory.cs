namespace WebPrintManager.Agent.Printing
{
    internal class ClientPrinterFactory
    {
        public IClientPrinter Create(string printerType, string printerData)
        {
            if (printerType is null)
            {
                throw new ArgumentNullException(nameof(printerType));
            }

            if (printerData is null)
            {
                throw new ArgumentNullException(nameof(printerData));
            }

            if (printerType == nameof(InstalledPrinter))
            {
                return System.Text.Json.JsonSerializer.Deserialize<InstalledPrinter>(printerData) !;
            }

            throw new InvalidOperationException("Invalid printer type");
        }
    }
}
