using System.Text;
using WebPrintManager.Agent.Printing.Win32;

namespace WebPrintManager.Agent.Services
{
    internal sealed class RawPrintService : IReceivedMessageCallback, IDisposable
    {
        private readonly ManagerSession session;
        private readonly Printing.PrintMonitor printMonitor;
        private readonly Printing.ClientPrinterFactory clientPrinterFactory;

        private readonly List<Printing.PrintJobInfo> jobs = new List<Printing.PrintJobInfo>();

        public RawPrintService(
            ManagerSession session,
            Printing.PrintMonitor printMonitor,
            Printing.ClientPrinterFactory clientPrinterFactory)
        {
            this.session = session;
            this.printMonitor = printMonitor;
            this.clientPrinterFactory = clientPrinterFactory;
            this.printMonitor.JobStatusChanged += this.PrintMonitorJobStatusChanged;
        }

        private void PrintMonitorJobStatusChanged(object sender, PrintJobChangeEventArgs e)
        {
            if (this.jobs.Any(f => f.Id == e.JobId))
            {
                this.session.SendJson(
                    new Messages.PrintJobStatusMessage(e.JobId, (Messages.PrintJobStatus)(int)e.JobStatus),
                    nameof(Messages.MessageType.PrintJobStatus).ToLowerInvariant(),
                    this.session.GenerateMessageTag(false),
                    false);
            }
        }

        private Printing.IRawPrinter GetRawPrinter(Printing.IClientPrinter printer)
        {
            if (printer is Printing.ISpoolerPrinter spoolerPrinter)
            {
                this.printMonitor.Configure(spoolerPrinter.SpoolerName);
                return new Printing.LocalRawPrinter();
            }

            throw new InvalidOperationException("Invalid printer type");
        }

        public void Execute(string tag, byte[] buffer, int offset, int size)
        {
            Printing.PrintJobInfo jobInfo;

            try
            {
                using (var inputStream = new MemoryStream(buffer, offset, size))
                {
                    var reader = new BinaryReader(inputStream, Encoding.UTF8);

                    var printerType = reader.ReadString();
                    var printerData = reader.ReadString();
                    var documentInfoData = reader.ReadString();
                    var dataLength = reader.ReadInt32();
                    var data = reader.ReadBytes(dataLength);
                    var clientPrinter = this.clientPrinterFactory.Create(printerType, printerData);
                    var rawPriner = this.GetRawPrinter(clientPrinter);
                    var documentInfo = System.Text.Json.JsonSerializer.Deserialize<Printing.PrintDocumentInfo>(documentInfoData) !;

                    jobInfo = rawPriner.Write(clientPrinter, documentInfo, data, 0, data.Length);

                    this.jobs.Add(jobInfo);
                }
            }
            catch (Exception ex)
            {
                this.session.SendJson(new Messages.RawPrintResult(false, ex.Message, 0), "message", tag, false);
                return;
            }

            this.session.SendJson(new Messages.RawPrintResult(true, null, jobInfo.Id), "message", tag, false);
        }

        public void Dispose()
        {
            this.printMonitor.JobStatusChanged -= this.PrintMonitorJobStatusChanged;
            GC.SuppressFinalize(this);
        }
    }
}
