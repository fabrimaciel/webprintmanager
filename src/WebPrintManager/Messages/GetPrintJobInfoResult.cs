namespace WebPrintManager.Messages
{
    internal class GetPrintJobInfoResult
    {
        public int JobId { get; set; }

        public PrintJobStatus Status { get; set; }

        public int TotalPages { get; set; }

        public int PagesPrinted { get; set; }
    }
}
