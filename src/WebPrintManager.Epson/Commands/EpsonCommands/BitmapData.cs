using System.Collections;

namespace WebPrintManager.Epson.Commands
{
    public class BitmapData
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public BitArray Dots { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        public int Height { get; set; }

        public int Width { get; set; }
    }
}