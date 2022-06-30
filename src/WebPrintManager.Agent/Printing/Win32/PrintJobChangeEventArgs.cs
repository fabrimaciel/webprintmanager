namespace WebPrintManager.Agent.Printing.Win32
{
    internal delegate void PrintJobStatusChanged(object sender, PrintJobChangeEventArgs e);

    internal class PrintJobChangeEventArgs : EventArgs
    {
        public int JobId { get; }

        public string JobName { get; }

        public JOBSTATUS JobStatus { get; }

        public PrintJobChangeEventArgs(int jobId, string strJobName, JOBSTATUS status)
        {
            this.JobId = jobId;
            this.JobName = strJobName;
            this.JobStatus = status;
        }
    }
}
