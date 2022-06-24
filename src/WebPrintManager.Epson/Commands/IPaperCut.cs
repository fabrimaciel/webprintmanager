namespace WebPrintManager.Epson.Commands
{
    internal interface IPaperCut
    {
        byte[] Full();
        byte[] Partial();
    }
}