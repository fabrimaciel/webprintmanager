namespace WebPrintManager.Epson.Commands
{
    internal interface IQrCode
    {
        byte[] Print(string qrData);
        byte[] Print(string qrData, QrCodeSize qrCodeSize);
    }
}