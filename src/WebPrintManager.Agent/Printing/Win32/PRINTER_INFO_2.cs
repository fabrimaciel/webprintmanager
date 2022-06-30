using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct PRINTER_INFO_2
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pServerName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrinterName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pShareName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPortName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDriverName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pComment;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pLocation;
        public IntPtr pDevMode;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pSepFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrintProcessor;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDatatype;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pParameters;
        public IntPtr pSecurityDescriptor;
        public uint Attributes;
        public uint Priority;
        public uint DefaultPriority;
        public uint StartTime;
        public uint UntilTime;
        public uint Status;
        public uint cJobs;
        public uint AveragePPM;
    }
#pragma warning enable
}
