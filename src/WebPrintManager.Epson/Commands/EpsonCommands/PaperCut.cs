namespace WebPrintManager.Epson.Commands
{
    public class PaperCut : IPaperCut
    {
        public byte[] Full()
        {
            return new byte[] { 29, 'V'.ToByte(), 65, 0 };
        }

        public byte[] Partial()
        {
            return new byte[] { 29, 'V'.ToByte(), 65, 1 };
        }
    }
}