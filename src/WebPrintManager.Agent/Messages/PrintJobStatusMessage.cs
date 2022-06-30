namespace WebPrintManager.Messages
{
    internal class PrintJobStatusMessage
    {
        public PrintJobStatusMessage(int jobId, PrintJobStatus status)
        {
            this.JobId = jobId;
            this.Status = status;
        }

        public int JobId { get; }

        public PrintJobStatus Status { get; }
    }
}
