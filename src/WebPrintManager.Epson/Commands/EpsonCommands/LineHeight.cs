namespace WebPrintManager.Epson.Commands
{
    internal class LineHeight : ILineHeight
    {
        public byte[] Normal()
        {
            return new byte[] { 27, '3'.ToByte(), 30 };
        }

        public byte[] SetLineHeight(byte height)
        {
            return new byte[] { 27, '3'.ToByte(), height };
        }
    }
}
