namespace WebPrintManager.Messages
{
    internal class GenericResult
    {
        public GenericResult(bool success, string? message)
        {
            this.Success = success;
            this.Message = message;
        }

        public bool Success { get; }

        public string? Message { get; }
    }
}
