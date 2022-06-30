namespace WebPrintManager.Agent.Printing
{
    internal class InstalledPrinter : IClientPrinter, ISpoolerPrinter
    {
        public string Name { get; set; }

        string ISpoolerPrinter.SpoolerName => this.Name;
    }
}
