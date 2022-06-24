namespace WebPrintManager.Epson.Commands
{
    internal interface IAlignment
    {
        byte[] Left();
        byte[] Right();
        byte[] Center();
    }
}