namespace WebPrintManager.Messages
{
    internal class ConnectionMessage
    {
        public ConnectionMessage()
        {
            this.Version = typeof(ConnectionMessage).Assembly.GetName().Version?.ToString();
            this.Connection = "CONNECTED";
        }

        public string? Version { get; set; }

        public string Connection { get; set; }
    }
}
