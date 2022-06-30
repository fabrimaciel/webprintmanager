using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    internal struct JOBINFO
    {
        public int JobId;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrinterName;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pMachineName;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pUserName;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDocument;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDatatype;

        [MarshalAs(UnmanagedType.LPTStr)]
        public string pStatus;

        public int Status;

        public int Priority;

        public int Position;

        public int TotalPages;

        public int PagesPrinted;

        public SYSTEMTIME Submitted;
    }
#pragma warning enable
}
