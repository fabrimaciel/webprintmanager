using System.ComponentModel;

namespace WebPrintManager
{
    public sealed class ClientPrintJob : System.ComponentModel.INotifyPropertyChanged, IDisposable
    {
        private readonly IClientPrinter printer;
        private readonly PrintManager printManager;

        private PrintJobStatus status;

        private Messages.GetPrintJobInfoResult jobInfo;

        public event PropertyChangedEventHandler? PropertyChanged;

        internal ClientPrintJob(
            Messages.GetPrintJobInfoResult jobInfo,
            IClientPrinter printer,
            PrintManager printManager)
        {
            this.jobInfo = jobInfo;
            this.printer = printer;
            this.printManager = printManager;

            this.status = jobInfo.Status;
            this.printManager.PrintJobStatusChanged += this.PrintManagerPrintJobStatusChanged;
        }

        ~ClientPrintJob()
        {
            this.Dispose();
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PrintManagerPrintJobStatusChanged(object? sender, PrintJobStatusChangedEventArgs e)
        {
            if (e.JobId == this.JobId)
            {
                var status1 = (PrintJobStatus)(int)e.Status;

                if (this.Status != status1)
                {
                    this.Status = status1;
                }
            }
        }

        public int JobId => this.jobInfo.JobId;

        public PrintJobStatus Status
        {
            get => this.status;
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.OnPropertyChanged(nameof(this.Status));
                }
            }
        }

        public Task Cancel(CancellationToken cancellationToken)
        {
            return this.printManager.CancelJob(this.printer, this.JobId, cancellationToken);
        }

        public async Task Refresh(CancellationToken cancellationToken)
        {
            var info = await this.printManager.GetJobInfo(this.printer, this.JobId, cancellationToken);
            PrintJobStatus status1;

            if (info != null)
            {
                this.jobInfo = info;
                status1 = info.Status;
            }
            else
            {
                status1 = PrintJobStatus.Completed;
            }

            if (this.Status != status1)
            {
                this.Status = status1;
            }
        }

        public void Dispose()
        {
            this.printManager.PrintJobStatusChanged -= this.PrintManagerPrintJobStatusChanged;
            GC.SuppressFinalize(this);
        }
    }
}