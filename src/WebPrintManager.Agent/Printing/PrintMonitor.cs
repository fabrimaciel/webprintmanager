using WebPrintManager.Agent.Printing.Win32;

namespace WebPrintManager.Agent.Printing
{
    internal sealed class PrintMonitor : IDisposable
    {
        private readonly List<PrintQueueMonitor> queueMonitors = new List<PrintQueueMonitor>();

        public event PrintJobStatusChanged? JobStatusChanged;

        public PrintQueueMonitor Configure(string spoolerName)
        {
            if (string.IsNullOrEmpty(spoolerName))
            {
                throw new ArgumentException($"'{nameof(spoolerName)}' cannot be null or empty.", nameof(spoolerName));
            }

            PrintQueueMonitor? queueMonitor;
            lock (this.queueMonitors)
            {
                queueMonitor = this.queueMonitors.FirstOrDefault(f => StringComparer.InvariantCultureIgnoreCase.Equals(spoolerName, f.SpoolerName));

                if (queueMonitor == null)
                {
                    queueMonitor = new PrintQueueMonitor(spoolerName);
                    queueMonitor.JobStatusChange += this.QueueMonitorJobStatusChange;
                    this.queueMonitors.Add(queueMonitor);
                }
                else
                {
                    return queueMonitor;
                }
            }

            queueMonitor.Start();
            return queueMonitor;
        }

        private void QueueMonitorJobStatusChange(object sender, PrintJobChangeEventArgs e)
        {
            this.JobStatusChanged?.Invoke(this, e);
        }

        public void Dispose()
        {
            foreach (var queue in this.queueMonitors)
            {
                queue.JobStatusChange -= this.QueueMonitorJobStatusChange;
                queue.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
