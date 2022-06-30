using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Explicit)]
    internal struct PRINTER_NOTIFY_INFO_DATA_UNION
    {
        [FieldOffset(0)]
        private uint adwData0;
        [FieldOffset(4)]
        private uint adwData1;
        [FieldOffset(0)]
        public PRINTER_NOTIFY_INFO_DATA_DATA Data;

        public uint[] adwData
        {
            get
            {
                return new uint[] { this.adwData0, this.adwData1 };
            }
        }
    }
#pragma warning enable
}
