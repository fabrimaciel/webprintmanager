namespace WebPrintManager.Epson.Commands
{
    internal interface IFontWidth
    {
        byte[] Normal();
        byte[] DoubleWidth2();
        byte[] DoubleWidth3();
    }
}