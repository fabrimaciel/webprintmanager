namespace WebPrintManager.Epson.Commands
{
    internal interface ILineHeight
    {
        byte[] Normal();
        byte[] SetLineHeight(byte height);
    }
}
