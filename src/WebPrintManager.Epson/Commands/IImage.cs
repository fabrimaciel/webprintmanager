using System.Drawing;

namespace WebPrintManager.Epson.Commands
{
    internal interface IImage
    {
        byte[] Print(Bitmap image);
    }
}
