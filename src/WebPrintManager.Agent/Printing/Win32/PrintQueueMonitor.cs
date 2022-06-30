using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
    internal sealed class PrintQueueMonitor : IDisposable
    {
        private const int ErrorInsufficientBuffer = 122;

        private readonly ManualResetEvent resetEvent = new ManualResetEvent(false);
        private readonly PRINTER_NOTIFY_OPTIONS notifyOptions = new PRINTER_NOTIFY_OPTIONS();

        private IntPtr printerHandle = IntPtr.Zero;
        private RegisteredWaitHandle waitHandle;
        private IntPtr changeHandle = IntPtr.Zero;

        public event PrintJobStatusChanged? JobStatusChange;

        public PrintQueueMonitor(string spoolerName)
        {
            this.SpoolerName = spoolerName;
            this.Start();
        }

        ~PrintQueueMonitor() => this.Dispose();

        public string SpoolerName { get; }

        public void Start()
        {
            var printerDefaults = new PRINTER_DEFAULTS();
            printerDefaults.DesiredAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
            NativeMethods.OpenPrinter(this.SpoolerName.Normalize(), out this.printerHandle, printerDefaults);
            if (this.printerHandle != IntPtr.Zero)
            {
                this.changeHandle = NativeMethods.FindFirstPrinterChangeNotification(
                    this.printerHandle,
                    (int)PRINTER_CHANGES.PRINTER_CHANGE_ALL,
                    0,
                    this.notifyOptions);

#pragma warning disable CS0618 // Type or member is obsolete
                this.resetEvent.Handle = this.changeHandle;
#pragma warning restore CS0618 // Type or member is obsolete
                this.waitHandle = ThreadPool.RegisterWaitForSingleObject(
                    this.resetEvent,
                    this.PrinterNotifyWaitCallback!,
                    this.resetEvent,
                    -1,
                    true);
            }
        }

        public void Stop()
        {
            if (this.printerHandle != IntPtr.Zero)
            {
                NativeMethods.ClosePrinter(this.printerHandle);
                this.printerHandle = IntPtr.Zero;
            }
        }

        public void PrinterNotifyWaitCallback(object state, bool timedOut)
        {
            if (this.printerHandle == IntPtr.Zero)
            {
                return;
            }

            this.notifyOptions.Count = 1;
            var pdwChange = 0;
            var result = NativeMethods.FindNextPrinterChangeNotification(
                this.changeHandle,
                out pdwChange,
                this.notifyOptions,
                out var notifyInfo);

            if (!result || notifyInfo == IntPtr.Zero)
            {
                return;
            }

            var jobRelatedChange = (pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB ||
                                   (pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_SET_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_SET_JOB ||
                                   (pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_DELETE_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_DELETE_JOB ||
                                   (pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_WRITE_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_WRITE_JOB;

            if (!jobRelatedChange)
            {
                return;
            }

            var info = (PRINTER_NOTIFY_INFO)Marshal.PtrToStructure(notifyInfo, typeof(PRINTER_NOTIFY_INFO)) !;
            long positionData = (long)notifyInfo + (long)Marshal.OffsetOf(typeof(PRINTER_NOTIFY_INFO), "aData");
            PRINTER_NOTIFY_INFO_DATA[] data = new PRINTER_NOTIFY_INFO_DATA[info.Count];
            for (uint i = 0; i < info.Count; i++)
            {
                data[i] = (PRINTER_NOTIFY_INFO_DATA)Marshal.PtrToStructure((IntPtr)positionData, typeof(PRINTER_NOTIFY_INFO_DATA)) !;
                positionData += Marshal.SizeOf(typeof(PRINTER_NOTIFY_INFO_DATA));
            }

            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i].Field == (ushort)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_STATUS &&
                    data[i].Type == (ushort)PRINTERNOTIFICATIONTYPES.JOB_NOTIFY_TYPE)
                {
                    var jobStatus = (JOBSTATUS)Enum.Parse(typeof(JOBSTATUS), data[i].NotifyData.Data.cbBuf.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    var jobId = (int)data[i].Id;
                    string strJobName = string.Empty;

                    this.JobStatusChange?.Invoke(this, new PrintJobChangeEventArgs(jobId, strJobName, jobStatus));
                }
            }

            this.resetEvent.Reset();
            this.waitHandle = ThreadPool.RegisterWaitForSingleObject(
                this.resetEvent,
                this.PrinterNotifyWaitCallback!,
                this.resetEvent,
                -1,
                true);
        }

        public JOB_INFO_2? GetJob(uint jobId)
        {
            uint needed;

            NativeMethods.GetJob(this.printerHandle, jobId, 2, IntPtr.Zero, 0, out needed);

            if (Marshal.GetLastWin32Error() != ErrorInsufficientBuffer)
            {
                Console.WriteLine("Get Job 1 failure, error code=" + Marshal.GetLastWin32Error());
            }
            else
            {
                var buffer = Marshal.AllocHGlobal((int)needed);
                NativeMethods.GetJob(this.printerHandle, jobId, 2, buffer, needed, out needed);
                var jobInfo = (JOB_INFO_2)Marshal.PtrToStructure(buffer, typeof(JOB_INFO_2)) !;
                Marshal.FreeHGlobal(buffer);

                return jobInfo;
            }

            return null;
        }

        public void CancelJob(uint jobId)
        {
            var result = NativeMethods.SetJob(this.printerHandle, jobId, 0u, IntPtr.Zero, (uint)JOBCONTROL.JOB_CONTROL_CANCEL);
            if (!result)
            {
                var lastError = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"Cancel job {jobId} from '{this.SpoolerName}'. Error: {lastError}");
            }
        }

        public PRINTER_INFO_2? GetPrinterInfo()
        {
            uint needed;

            NativeMethods.GetPrinter(this.printerHandle, 2, IntPtr.Zero, 0, out needed);

            if (Marshal.GetLastWin32Error() != ErrorInsufficientBuffer)
            {
                Console.WriteLine("Get Job 1 failure, error code=" + Marshal.GetLastWin32Error());
            }
            else
            {
                var buffer = Marshal.AllocHGlobal((int)needed);
                NativeMethods.GetPrinter(this.printerHandle, 2, buffer, needed, out needed);
                var info = (PRINTER_INFO_2)Marshal.PtrToStructure(buffer, typeof(PRINTER_INFO_2)) !;
                Marshal.FreeHGlobal(buffer);

                return info;
            }

            return null;
        }

        public void Purge()
        {
            var result = NativeMethods.SetPrinter(this.printerHandle, 0u, IntPtr.Zero, 3u);

            if (!result)
            {
                var lastError = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"Purge printer '{this.SpoolerName}'. Error: {lastError}");
            }
        }

        public void Dispose()
        {
            this.Stop();
            this.resetEvent.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
