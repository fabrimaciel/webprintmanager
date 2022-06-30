namespace WebPrintManager.Messages
{
    internal class RawPrintResult
    {
        public RawPrintResult()
        {
        }

        public RawPrintResult(bool success, string? error, int jobId)
        {
            this.Success = success;
            this.Error = error;
            this.JobId = jobId;
        }

        public bool Success { get; set; }

        public string? Error { get; set; }

        public int JobId { get; set; }
    }
}
