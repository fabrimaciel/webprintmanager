using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class PRINTER_DEFAULTS
    {
        public IntPtr pDatatype;

        public IntPtr pDevMode;

        public ACCESS_MASK DesiredAccess;
    }
#pragma warning enable
}
