using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct JOB_INFO_1
    {
        public uint JobId;
        public string pPrinterName;
        public string pMachineName;
        public string pUserName;
        public string pDocument;
        public string pDatatype;
        public string pStatus;
        public uint Status;
        public uint Priority;
        public uint Position;
        public uint TotalPages;
        public uint PagesPrinted;
        public SYSTEMTIME Submitted;
    }
#pragma warning enable
}
