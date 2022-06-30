using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential)]
    internal struct PRINTER_NOTIFY_INFO
    {
        public uint Version;
        public uint Flags;
        public uint Count;
        public PRINTER_NOTIFY_INFO_DATA_UNION aData;
    }
#pragma warning enable
}
