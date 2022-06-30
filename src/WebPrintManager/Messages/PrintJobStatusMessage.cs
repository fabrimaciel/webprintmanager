namespace WebPrintManager.Messages
{
    internal class PrintJobStatusMessage
    {
        public int JobId { get; set; }

        public PrintJobStatus Status { get; set; }
    }
}
