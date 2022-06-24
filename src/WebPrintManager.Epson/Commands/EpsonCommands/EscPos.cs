namespace WebPrintManager.Epson.Commands
{
    internal class EscPos : IPrintCommand
    {
        public IFontMode FontMode { get; set; }
        public IFontWidth FontWidth { get; set; }
        public IAlignment Alignment { get; set; }
        public IPaperCut PaperCut { get; set; }
        public IDrawer Drawer { get; set; }
        public IQrCode QrCode { get; set; }
        public IBarCode BarCode { get; set; }
        public IInitializePrint InitializePrint { get; set; }
        public IImage Image { get; set; }
        public ILineHeight LineHeight { get; set; }
        public int ColsNomal => 48;
        public int ColsCondensed => 64;
        public int ColsExpanded => 24;

        public EscPos()
        {
            this.FontMode = new FontMode();
            this.FontWidth = new FontWidth();
            this.Alignment = new Alignment();
            this.PaperCut = new PaperCut();
            this.Drawer = new Drawer();
            this.QrCode = new QrCode();
            this.BarCode = new BarCode();
            this.Image = new Image();
            this.LineHeight = new LineHeight();
            this.InitializePrint = new InitializePrint();
        }

        public byte[] Separator(char speratorChar = '-')
        {
            return this.FontMode.Condensed(PrinterModeState.On)
                .AddBytes(new string(speratorChar, this.ColsCondensed))
                .AddBytes(this.FontMode.Condensed(PrinterModeState.Off))
                .AddCrLF();
        }

        public byte[] AutoTest()
        {
            return new byte[] { 29, 40, 65, 2, 0, 0, 2 };
        }
    }
}