using System.Runtime.InteropServices;
using WebPrintManager.Agent.Printing.Win32;

namespace WebPrintManager.Agent.Printing
{
    internal class LocalRawPrinter : IRawPrinter
    {
        public static PrintJobInfo SendFileToPrinter(string printerName, PrintDocumentInfo documentInfo, string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);

            var br = new BinaryReader(fs);

            var length = Convert.ToInt32(fs.Length);

            var bytes = br.ReadBytes(length);
            var unmanagedBytes = Marshal.AllocCoTaskMem(length);
            Marshal.Copy(bytes, 0, unmanagedBytes, length);
            var success = SendBytesToPrinter(printerName, documentInfo, unmanagedBytes, length);
            Marshal.FreeCoTaskMem(unmanagedBytes);
            return success;
        }

        public static PrintJobInfo SendBytesToPrinter(string printerName, PrintDocumentInfo documentInfo, IntPtr bytes, int count)
        {
            var di = new DocInfoA();
            var success = false;

            di.pDocName = documentInfo.Name;
            di.pDataType = "RAW";

            var printerDefaults = new PRINTER_DEFAULTS();
            if (NativeMethods.OpenPrinter(printerName.Normalize(), out var printer, printerDefaults))
            {
                var jobId = NativeMethods.StartDocPrinter(printer, 1, di);
                if (jobId > 0)
                {
                    if (NativeMethods.StartPagePrinter(printer))
                    {
                        success = NativeMethods.WritePrinter(printer, bytes, count, out var _);
                        NativeMethods.EndPagePrinter(printer);
                    }

                    NativeMethods.EndDocPrinter(printer);
                }

                NativeMethods.ClosePrinter(printer);

                if (!success)
                {
                    throw new InvalidOperationException("Error on write document to printer.");
                }

                return new PrintJobInfo(jobId, printerName);
            }
            else
            {
                throw new InvalidOperationException($"Error on open printer {printerName}.");
            }
        }

        public static PrintJobInfo SendBytesToPrinter(string printerName, PrintDocumentInfo documentInfo, byte[] data)
        {
            var unmanagedBytes = Marshal.AllocCoTaskMem(data.Length);
            Marshal.Copy(data, 0, unmanagedBytes, data.Length);
            var retval = SendBytesToPrinter(printerName, documentInfo, unmanagedBytes, data.Length);
            Marshal.FreeCoTaskMem(unmanagedBytes);
            return retval;
        }

        public static PrintJobInfo SendStringToPrinter(string printerName, PrintDocumentInfo documentInfo, string text)
        {
            var count = text.Length;

            var bytes = Marshal.StringToCoTaskMemAnsi(text);

            var result = SendBytesToPrinter(printerName, documentInfo, bytes, count);
            Marshal.FreeCoTaskMem(bytes);

            return result;
        }

        public PrintJobInfo Write(IClientPrinter printer, PrintDocumentInfo documentInfo, byte[] data, int offset, int count)
        {
            var installedPrinter = (InstalledPrinter)printer;

            var unmanagedBytes = Marshal.AllocCoTaskMem(count);
            Marshal.Copy(data, offset, unmanagedBytes, count);
            var retval = SendBytesToPrinter(installedPrinter.Name, documentInfo, unmanagedBytes, data.Length);
            Marshal.FreeCoTaskMem(unmanagedBytes);
            return retval;
        }
    }
}
