using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential)]
    internal struct PRINTER_NOTIFY_INFO_DATA_DATA
    {
        public uint cbBuf;
        public IntPtr pBuf;
    }
#pragma warning enable
}
