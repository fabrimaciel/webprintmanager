namespace WebPrintManager
{
    public class InstalledPrinter : IClientPrinter
    {
        public InstalledPrinter()
        {
        }

        public InstalledPrinter(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
