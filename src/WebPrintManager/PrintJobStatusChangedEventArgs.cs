namespace WebPrintManager
{
    internal class PrintJobStatusChangedEventArgs : EventArgs
    {
        public PrintJobStatusChangedEventArgs(int jobId, PrintJobStatus status)
        {
            this.JobId = jobId;
            this.Status = status;
        }

        public int JobId { get; }

        public PrintJobStatus Status { get; }
    }
}