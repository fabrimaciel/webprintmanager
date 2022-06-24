namespace WebPrintManager.Messages
{
    internal class RawPrintResult
    {
        public RawPrintResult()
        {
        }

        public RawPrintResult(bool success, string? error)
        {
            this.Success = success;
            this.Error = error;
        }

        public bool Success { get; set; }

        public string? Error { get; set; }
    }
}
