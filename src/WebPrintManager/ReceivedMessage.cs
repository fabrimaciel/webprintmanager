namespace WebPrintManager
{
    internal class ReceivedMessage
    {
        public ReceivedMessage(string? messageTag, Messages.MessageType type, string response)
        {
            this.Tag = messageTag;
            this.Type = type;
            this.Response = response;
        }

        public string? Tag { get; }

        public Messages.MessageType Type { get; }

        public string Response { get; }
    }
}