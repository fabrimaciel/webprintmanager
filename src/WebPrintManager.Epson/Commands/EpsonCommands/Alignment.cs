﻿namespace WebPrintManager.Epson.Commands
{
    public class Alignment : IAlignment
    {
        public byte[] Left()
        {
            return new byte[] { 27, 'a'.ToByte(), 0 };
        }

        public byte[] Right()
        {
            return new byte[] { 27, 'a'.ToByte(), 2 };
        }

        public byte[] Center()
        {
            return new byte[] { 27, 'a'.ToByte(), 1 };
        }
    }
}