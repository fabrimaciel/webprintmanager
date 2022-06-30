using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
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
            PRINTER_DEFAULTS pDefault);

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
        public static extern int StartDocPrinter(
            IntPtr hPrinter,
            int level,
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

        [DllImport(
            "winspool.Drv",
            EntryPoint = "FindFirstPrinterChangeNotification",
            SetLastError = true, CharSet = CharSet.Ansi,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindFirstPrinterChangeNotification
            ([In()] IntPtr hPrinter,
            [In()] int fwFlags,
            [In()] int fwOptions,
            [In(),
                MarshalAs(UnmanagedType.LPStruct)]
                PRINTER_NOTIFY_OPTIONS pPrinterNotifyOptions);

        [DllImport(
            "winspool.Drv",
            EntryPoint = "FindNextPrinterChangeNotification",
            SetLastError = true,
            CharSet = CharSet.Ansi,
            ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool FindNextPrinterChangeNotification
            ([In()] IntPtr hChangeObject,
            [Out()] out int pdwChange,
            [In(),
            MarshalAs(UnmanagedType.LPStruct)]
            PRINTER_NOTIFY_OPTIONS pPrinterNotifyOptions,
            [Out()] out IntPtr lppPrinterNotifyInfo);

        [DllImport(
            "winspool.Drv",
            CharSet = CharSet.Unicode,
            EntryPoint = "GetJob",
            SetLastError = true)]
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern bool GetJob(
            [In] IntPtr hPrinter,
            [In] uint jobId,
            [In] uint level,
            [Out] IntPtr pJob,
            [In] uint cbBuf,
            [Out] out uint pcbNeeded);

        [DllImport(
            "winspool.Drv",
            CharSet = CharSet.Unicode,
            EntryPoint = "SetJob",
            SetLastError = true)]
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern bool SetJob(
            [In()] IntPtr hPrinter,
            [In()] uint jobId,
            [In()] uint level,
            [In()] IntPtr pJob,
            [In()] uint command);

        [DllImport(
           "winspool.Drv",
            CharSet = CharSet.Unicode,
           EntryPoint = "GetPrinter",
           SetLastError = true)]
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern bool GetPrinter(
            [In] IntPtr hPrinter,
            [In] uint dwLevel,
            [Out] IntPtr pPrinter,
            [In] uint dwBuf,
            [Out] out uint dwNeeded);

        [DllImport(
           "winspool.Drv",
            CharSet = CharSet.Unicode,
           EntryPoint = "SetPrinter",
           SetLastError = true)]
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern bool SetPrinter(
            [In] IntPtr hPrinter,
            [In] uint level,
            [In] IntPtr pPrinter,
            [In] uint command);
    }
#pragma warning restore
}
