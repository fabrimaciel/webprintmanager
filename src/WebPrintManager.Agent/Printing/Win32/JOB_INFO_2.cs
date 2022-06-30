using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct JOB_INFO_2
    {
        public UInt32 JobId;
        public IntPtr pPrinterName;
        public IntPtr pMachineName;
        public IntPtr pUserName;
        public IntPtr pDocument;
        public IntPtr pNotifyName;
        public IntPtr pDatatype;
        public IntPtr pPrintProcessor;
        public IntPtr pParameters;
        public IntPtr pDriverName;
        public IntPtr pDevMode;
        public IntPtr pStatus;
        public IntPtr pSecurityDescriptor;
        public UInt32 Status;
        public UInt32 Priority;
        public UInt32 Position;
        public UInt32 StartTime;
        public UInt32 UntilTime;
        public UInt32 TotalPages;
        public UInt32 Size;
        public SYSTEMTIME Submitted;
        public UInt32 Time;
        public UInt32 PagesPrinted;
    }
#pragma warning enable
}
