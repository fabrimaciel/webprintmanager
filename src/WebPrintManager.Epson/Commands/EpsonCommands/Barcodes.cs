namespace WebPrintManager.Epson.Commands
{
    internal static class Barcodes
    {
        public const byte PrintBarcode = 0x6B;
        public const byte SetBarcodeHeightInDots = 0x68;
        public const byte SetBarWidth = 0x77;
        public const byte SetBarLabelPosition = 0x48;
        public const byte SetBarLabelFont = 0x66;

        public const byte Set2DCode = 0x28;
        public const byte AutoEnding = 0x00;
        public static readonly byte[] SetPDF417NumberOfColumns = { 0x03, 0x00, 0x30, 0x41 };
        public static readonly byte[] SetPDF417NumberOfRows = { 0x03, 0x00, 0x30, 0x42 };
        public static readonly byte[] SetPDF417DotSize = { 0x03, 0x00, 0x30, 0x43 };
        public static readonly byte[] SetPDF417CorrectionLevel = { 0x04, 0x00, 0x30, 0x45, 0x30 };
        public static readonly byte[] StorePDF417Data = { 0x00, 0x30, 0x50, 0x30 };
        public static readonly byte[] PrintPDF417 = { 0x03, 0x00, 0x30, 0x51, 0x30 };

        public static readonly byte[] SelectQRCodeModel = { 0x04, 0x00, 0x31, 0x41 };
        public static readonly byte[] SetQRCodeDotSize = { 0x03, 0x00, 0x31, 0x43 };
        public static readonly byte[] SetQRCodeCorrectionLevel = { 0x03, 0x00, 0x31, 0x45 };
        public static readonly byte[] StoreQRCodeData = { 0x31, 0x50, 0x30 };
        public static readonly byte[] PrintQRCode = { 0x03, 0x00, 0x31, 0x51, 0x30 };
    }
}
