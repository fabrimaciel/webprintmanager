namespace WebPrintManager.Agent.Printing
{
    internal class PrintJobInfo
    {
        public PrintJobInfo(int id, string spoolerName)
        {
            this.Id = id;
            this.SpoolerName = spoolerName;
        }

        public int Id { get; }

        public string SpoolerName { get; }
    }
}
