namespace WebPrintManager.Agent
{
    internal class ManagerServerOptions
    {
        public const string ManagerServer = "ManagerServer";

        public string Address { get; set; } = "0.0.0.0";

        public int Port { get; set; } = 24080;

        public List<string> AuthorizedSites { get; set; }
    }
}
