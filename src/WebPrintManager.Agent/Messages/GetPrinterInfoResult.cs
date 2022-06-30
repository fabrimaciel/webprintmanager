namespace WebPrintManager.Messages
{
    internal class GetPrinterInfoResult
    {
        public GetPrinterInfoResult(PrintQueueStatus status)
        {
            this.Status = status;
        }

        public PrintQueueStatus Status { get; }
    }
}
