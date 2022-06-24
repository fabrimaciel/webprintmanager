namespace WebPrintManager.Epson.Commands
{
    internal interface IBarCode
    {
        byte[] Code128(string code, Positions printString);
        byte[] Code39(string code, Positions printString);
        byte[] Ean13(string code, Positions printString);
    }
}