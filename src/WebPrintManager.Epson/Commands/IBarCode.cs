namespace WebPrintManager.Epson.Commands
{
    internal interface IBarCode
    {
        byte[] Code128(
            string text,
            Positions positions,
            BarWidth width,
            int height,
            bool useFontB,
            BarcodeCode code);

        byte[] Code39(
            string text,
            Positions positions,
            BarWidth width,
            int height,
            bool useFontB);

        byte[] Ean13(
            string text,
            Positions positions,
            BarWidth width,
            int height,
            bool useFontB);
    }
}