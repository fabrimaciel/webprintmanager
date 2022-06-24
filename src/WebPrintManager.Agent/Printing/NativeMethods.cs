using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing
{
#pragma warning disable
    internal static class NativeMethods
    {
        [DllImport(
            "winspool.Drv",
            EntryPoint = "OpenPrinterA",
            SetLastError = true,
            CharSet = CharSet.Ansi,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter(
            [MarshalAs(UnmanagedType.LPStr)] string szPrinter,
            out IntPtr hPrinter,
            IntPtr pd);

        [DllImport(
            "winspool.Drv",
            EntryPoint = "ClosePrinter",
            SetLastError = true,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport(
            "winspool.Drv",
            EntryPoint = "StartDocPrinterA",
            SetLastError = true,
            CharSet = CharSet.Ansi,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level,
            [In][MarshalAs(UnmanagedType.LPStruct)] DocInfoA di);

        [DllImport(
            "winspool.Drv",
            EntryPoint = "EndDocPrinter",
            SetLastError = true,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport(
            "winspool.Drv",
            EntryPoint = "StartPagePrinter",
            SetLastError = true,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport(
            "winspool.Drv",
            EntryPoint = "EndPagePrinter",
            SetLastError = true,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport(
            "winspool.Drv",
            EntryPoint = "WritePrinter",
            SetLastError = true,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);
    }
#pragma warning restore
}
