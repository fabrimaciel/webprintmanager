using System.Globalization;
using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential)]
    internal struct SYSTEMTIME
    {
        [MarshalAs(UnmanagedType.U2)]
        public short Year;
        [MarshalAs(UnmanagedType.U2)]
        public short Month;
        [MarshalAs(UnmanagedType.U2)]
        public short DayOfWeek;
        [MarshalAs(UnmanagedType.U2)]
        public short Day;
        [MarshalAs(UnmanagedType.U2)]
        public short Hour;
        [MarshalAs(UnmanagedType.U2)]
        public short Minute;
        [MarshalAs(UnmanagedType.U2)]
        public short Second;
        [MarshalAs(UnmanagedType.U2)]
        public short Milliseconds;

        public SYSTEMTIME(DateTime dt)
        {
            dt = dt.ToUniversalTime();  // SetSystemTime expects the SYSTEMTIME in UTC
            this.Year = (short)dt.Year;
            this.Month = (short)dt.Month;
            this.DayOfWeek = (short)dt.DayOfWeek;
            this.Day = (short)dt.Day;
            this.Hour = (short)dt.Hour;
            this.Minute = (short)dt.Minute;
            this.Second = (short)dt.Second;
            this.Milliseconds = (short)dt.Millisecond;
        }

        public DateTime ToDateTime()
        {
            return new DateTime(
                this.Year,
                this.Month,
                this.Day,
                this.Hour,
                this.Minute,
                this.Second,
                this.Milliseconds,
                CultureInfo.CurrentCulture.Calendar,
                DateTimeKind.Utc).ToLocalTime();
        }
    }
#pragma warning enable
}
