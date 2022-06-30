using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential)]
    internal class PRINTER_NOTIFY_OPTIONS
    {
        public int dwVersion = 2;
        public int dwFlags;
        public int Count = 2;
        public IntPtr lpTypes;

        public PRINTER_NOTIFY_OPTIONS()
        {
            int bytesNeeded = (2 + PRINTER_NOTIFY_OPTIONS_TYPE.JOB_FIELDS_COUNT + PRINTER_NOTIFY_OPTIONS_TYPE.PRINTER_FIELDS_COUNT) * 2;
            var pJobTypes = new PRINTER_NOTIFY_OPTIONS_TYPE();
            this.lpTypes = Marshal.AllocHGlobal(bytesNeeded);
            Marshal.StructureToPtr(pJobTypes, this.lpTypes, true);
        }
    }
#pragma warning enable
}