using System;

namespace WebPrintManager.Epson.Commands
{
    public class BarCode : IBarCode
    {
        public byte[] Code128(string code, Positions printString)
        {
            return new byte[] { 29, 119, 2 } // Width
                .AddBytes(new byte[] { 29, 104, 50 }) // Height
                .AddBytes(new byte[] { 29, 102, 1 }) // font hri character
                .AddBytes(new byte[] { 29, 72, printString.ToByte() }) // If print code informed
                .AddBytes(new byte[] { 29, 107, 73 }) // printCode
                .AddBytes(new[] { (byte)(code.Length + 2) })
                .AddBytes(new[] { '{'.ToByte(), 'C'.ToByte() })
                .AddBytes(code)
                .AddLF();
        }

        public byte[] Code39(string code, Positions printString)
        {
            return new byte[] { 29, 119, 2 } // Width
                .AddBytes(new byte[] { 29, 104, 50 }) // Height
                .AddBytes(new byte[] { 29, 102, 0 }) // font hri character
                .AddBytes(new byte[] { 29, 72, printString.ToByte() }) // If print code informed
                .AddBytes(new byte[] { 29, 107, 4 })
                .AddBytes(code)
                .AddBytes(new byte[] { 0 })
                .AddLF();
        }

        public byte[] Ean13(string code, Positions printString)
        {
            if (code.Trim().Length != 13)
            {
                return Array.Empty<byte>();
            }

            return new byte[] { 29, 119, 2 } // Width
                .AddBytes(new byte[] { 29, 104, 50 }) // Height
                .AddBytes(new byte[] { 29, 72, printString.ToByte() }) // If print code informed
                .AddBytes(new byte[] { 29, 107, 67, 12 })
                .AddBytes(code.Substring(0, 12))
                .AddLF();
        }
    }
}