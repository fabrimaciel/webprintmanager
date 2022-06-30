using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential)]
    internal struct PRINTER_NOTIFY_INFO_DATA
    {
        public ushort Type;
        public ushort Field;
        public uint Reserved;
        public uint Id;
        public PRINTER_NOTIFY_INFO_DATA_UNION NotifyData;
    }
#pragma warning enable
}
