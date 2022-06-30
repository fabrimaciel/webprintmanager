using WebPrintManager.Messages;

namespace WebPrintManager
{
    public class PrinterInfo
    {
        private readonly GetPrinterInfoResult? result;

        internal PrinterInfo(GetPrinterInfoResult? result)
        {
            this.result = result;
        }
    }
}