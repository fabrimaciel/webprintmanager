using System;
using System.Collections;
using System.Drawing;

namespace WebPrintManager.Epson.Commands
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class Image : IImage
    {
        private static BitmapData GetBitmapData(Bitmap bmp)
        {
            var threshold = 127;
            var index = 0;
            var multiplier = 576.0; // this depends on your printer model.
            var scale = multiplier / bmp.Width;
            int xheight = (int)(bmp.Height * scale);
            int xwidth = (int)(bmp.Width * scale);
            var dimensions = xwidth * xheight;
            var dots = new BitArray(dimensions);

            for (var y = 0; y < xheight; y++)
            {
                for (var x = 0; x < xwidth; x++)
                {
                    var x2 = (int)(x / scale);
                    var y2 = (int)(y / scale);
                    var color = bmp.GetPixel(x2, y2);
                    var luminance = (int)((color.R * 0.3) + (color.G * 0.59) + (color.B * 0.11));
                    dots[index] = luminance < threshold;
                    index++;
                }
            }

            return new BitmapData()
            {
                Dots = dots,
                Height = (int)(bmp.Height * scale),
                Width = (int)(bmp.Width * scale),
            };
        }

        byte[] IImage.Print(Bitmap image)
        {
            var data = GetBitmapData(image);
            var dots = data.Dots;
            var width = BitConverter.GetBytes(data.Width);

            int offset = 0;
            var stream = new System.IO.MemoryStream();
            var bw = new System.IO.BinaryWriter(stream);

            bw.Write((char)0x1B);
            bw.Write('@');

            bw.Write((char)0x1B);
            bw.Write('3');
            bw.Write((byte)24);

            while (offset < data.Height)
            {
                bw.Write((char)0x1B);
                bw.Write('*');         // bit-image mode
                bw.Write((byte)33);    // 24-dot double-density
                bw.Write(width[0]);  // width low byte
                bw.Write(width[1]);  // width high byte

                for (int x = 0; x < data.Width; ++x)
                {
                    for (int k = 0; k < 3; ++k)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; ++b)
                        {
                            int y = (((offset / 8) + k) * 8) + b;

                            // Calculate the location of the pixel we want in the bit array.
                            // It'll be at (y * width) + x.
                            int i = (y * data.Width) + x;

                            // If the image is shorter than 24 dots, pad with zero.
                            bool v = false;
                            if (i < dots.Length)
                            {
                                v = dots[i];
                            }

                            slice |= (byte)((v ? 1 : 0) << (7 - b));
                        }

                        bw.Write(slice);
                    }
                }

                offset += 24;
                bw.Write((char)0x0A);
            }

            // Restore the line spacing to the default of 30 dots.
            bw.Write((char)0x1B);
            bw.Write('3');
            bw.Write((byte)30);

            bw.Flush();
            byte[] bytes = stream.ToArray();
            bw.Dispose();
            return bytes;
        }
    }
#pragma warning restore CA1416 // Validate platform compatibility
}