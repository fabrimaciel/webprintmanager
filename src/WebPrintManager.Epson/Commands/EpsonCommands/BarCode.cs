using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebPrintManager.Epson.Commands
{
    public class BarCode : IBarCode
    {
        public byte[] BarcodeBytes(BarcodeType type, string barcode, BarcodeCode code)
        {
            if (type == BarcodeType.CODE128)
            {
                if (code == BarcodeCode.CodeC)
                {
                    byte[] b = Encoding.ASCII.GetBytes(barcode);
                    byte[] ob = new byte[b.Length / 2];
                    for (int i = 0, obc = 0; i < b.Length; i += 2)
                    {
                        ob[obc++] = (byte)(((b[i] - '0') * 10) + (b[i + 1] - '0'));
                    }

                    barcode = Encoding.ASCII.GetString(ob);
                }

                barcode = barcode.Replace("{", "{{");
                barcode = $"{(char)0x7B}{(char)code}" + barcode;
            }

            var command = new List<byte> { Cmd.GS, Barcodes.PrintBarcode, (byte)type, (byte)barcode.Length };
            command.AddRange(barcode.ToCharArray().Select(x => (byte)x));
            return command.ToArray();
        }

        public byte[] SetBarcodeHeightInDots(int height) => new byte[] { Cmd.GS, Barcodes.SetBarcodeHeightInDots, (byte)height };

        public byte[] SetBarWidth(BarWidth width) => new byte[] { Cmd.GS, Barcodes.SetBarWidth, (byte)width };

        public byte[] SetBarLabelPosition(Positions position) => new byte[] { Cmd.GS, Barcodes.SetBarLabelPosition, (byte)position };

        public byte[] SetBarLabelFontB(bool fontB) => new byte[] { Cmd.GS, Barcodes.SetBarLabelFont, (byte)(fontB ? 1 : 0) };

        public byte[] Code128(string text, Positions positions, BarWidth width, int height, bool useFontB, BarcodeCode code)
        {
            return this
                .SetBarWidth(width)
                .AddBytes(this.SetBarcodeHeightInDots(height))
                .AddBytes(this.SetBarLabelFontB(useFontB))
                .AddBytes(this.SetBarLabelPosition(positions))
                .AddBytes(this.BarcodeBytes(BarcodeType.CODE128, text, code))
                .AddLF();
        }

        public byte[] Code39(string text, Positions positions, BarWidth width, int height, bool useFontB)
        {
            return this
                .SetBarWidth(width)
                .AddBytes(this.SetBarcodeHeightInDots(height))
                .AddBytes(this.SetBarLabelFontB(useFontB))
                .AddBytes(this.SetBarLabelPosition(positions))
                .AddBytes(this.BarcodeBytes(BarcodeType.CODE39, text, BarcodeCode.CodeB))
                .AddLF();
        }

        public byte[] Ean13(string text, Positions positions, BarWidth width, int height, bool useFontB)
        {
            return this
                .SetBarWidth(width)
                .AddBytes(this.SetBarcodeHeightInDots(height))
                .AddBytes(this.SetBarLabelFontB(useFontB))
                .AddBytes(this.SetBarLabelPosition(positions))
                .AddBytes(this.BarcodeBytes(BarcodeType.JAN13_EAN13, text, BarcodeCode.CodeB))
                .AddLF();
        }
    }
}