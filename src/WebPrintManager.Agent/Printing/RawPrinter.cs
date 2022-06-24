using System.Runtime.InteropServices;
using System.Text;

namespace WebPrintManager.Agent.Printing
{
    internal static class RawPrinter
    {
        public static bool SendFileToPrinter(string printerName, string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);

            var br = new BinaryReader(fs);

            var length = Convert.ToInt32(fs.Length);

            var bytes = br.ReadBytes(length);
            var unmanagedBytes = Marshal.AllocCoTaskMem(length);
            Marshal.Copy(bytes, 0, unmanagedBytes, length);
            var success = SendBytesToPrinter(printerName, unmanagedBytes, length);
            Marshal.FreeCoTaskMem(unmanagedBytes);
            return success;
        }

        public static bool SendBytesToPrinter(string printerName, IntPtr bytes, int count)
        {
            var di = new DocInfoA();
            var success = false;

            di.pDocName = "VIP RAW PrinterDocument";
            di.pDataType = "RAW";

            if (NativeMethods.OpenPrinter(printerName.Normalize(), out var printer, IntPtr.Zero))
            {
                if (NativeMethods.StartDocPrinter(printer, 1, di))
                {
                    if (NativeMethods.StartPagePrinter(printer))
                    {
                        success = NativeMethods.WritePrinter(printer, bytes, count, out var _);
                        NativeMethods.EndPagePrinter(printer);
                    }

                    NativeMethods.EndDocPrinter(printer);
                }

                NativeMethods.ClosePrinter(printer);
            }

            return success;
        }

        public static bool SendBytesToPrinter(string printerName, byte[] data)
        {
            var unmanagedBytes = Marshal.AllocCoTaskMem(data.Length);
            Marshal.Copy(data, 0, unmanagedBytes, data.Length);
            var retval = SendBytesToPrinter(printerName, unmanagedBytes, data.Length);
            Marshal.FreeCoTaskMem(unmanagedBytes);

            return retval;
        }

        public static bool SendStringToPrinter(string printerName, string text)
        {
            var count = text.Length;

            var bytes = Marshal.StringToCoTaskMemAnsi(text);

            var result = SendBytesToPrinter(printerName, bytes, count);
            Marshal.FreeCoTaskMem(bytes);

            return result;
        }

        public static bool SendAsciiToPrinter(string printerName, string data)
        {
            var retval = SendBytesToPrinter(printerName, Encoding.ASCII.GetBytes(data));

            return retval;
        }
    }
}
