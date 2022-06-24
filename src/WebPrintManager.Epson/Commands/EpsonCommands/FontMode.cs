namespace WebPrintManager.Epson.Commands
{
    public class FontMode : IFontMode
    {
        public byte[] Bold(string value)
        {
            return this.Bold(PrinterModeState.On)
                .AddBytes(value)
                .AddBytes(this.Bold(PrinterModeState.Off))
                .AddLF();
        }

        public byte[] Bold(PrinterModeState state)
        {
            return state == PrinterModeState.On
                ? new byte[] { 27, 'E'.ToByte(), 1 }
                : new byte[] { 27, 'E'.ToByte(), 0 };
        }

        public byte[] Underline(string value)
        {
            return this.Underline(PrinterModeState.On)
                .AddBytes(value)
                .AddBytes(this.Underline(PrinterModeState.Off))
                .AddLF();
        }

        public byte[] Underline(PrinterModeState state)
        {
            return state == PrinterModeState.On
                ? new byte[] { 27, '-'.ToByte(), 1 }
                : new byte[] { 27, '-'.ToByte(), 0 };
        }

        public byte[] Expanded(string value)
        {
            return this.Expanded(PrinterModeState.On)
                .AddBytes(value)
                .AddBytes(this.Expanded(PrinterModeState.Off))
                .AddLF();
        }

        public byte[] Expanded(PrinterModeState state)
        {
            return state == PrinterModeState.On
                ? new byte[] { 29, '!'.ToByte(), 16 }
                : new byte[] { 29, '!'.ToByte(), 0 };
        }

        public byte[] Condensed(string value)
        {
            return this.Condensed(PrinterModeState.On)
                .AddBytes(value)
                .AddBytes(this.Condensed(PrinterModeState.Off))
                .AddLF();
        }

        public byte[] Condensed(PrinterModeState state)
        {
            return state == PrinterModeState.On
                ? new byte[] { 27, '!'.ToByte(), 1 }
                : new byte[] { 27, '!'.ToByte(), 0 };
        }

        public byte[] Font(string value, Fonts state)
        {
            return this.Font(state)
            .AddBytes(value)
            .AddBytes(this.Font(Fonts.FontA))
            .AddLF();
        }

        public byte[] Font(Fonts state)
        {
            byte fnt = 0;
            switch (state)
            {
                case Fonts.FontA:
                    {
                        fnt = 0;
                        break;
                    }

                case Fonts.FontB:
                    {
                        fnt = 1;
                        break;
                    }

                case Fonts.FontC:
                    {
                        fnt = 2;
                        break;
                    }

                case Fonts.FontD:
                    {
                        fnt = 3;
                        break;
                    }

                case Fonts.FontE:
                    {
                        fnt = 4;
                        break;
                    }

                case Fonts.SpecialFontA:
                    {
                        fnt = 5;
                        break;
                    }

                case Fonts.SpecialFontB:
                    {
                        fnt = 6;
                        break;
                    }
            }

            return new byte[] { 27, 'M'.ToByte(), fnt };
        }
    }
}