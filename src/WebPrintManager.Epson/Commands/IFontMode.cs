namespace WebPrintManager.Epson.Commands
{
    internal interface IFontMode
    {
        byte[] Bold(string value);
        byte[] Bold(PrinterModeState state);
        byte[] Underline(string value);
        byte[] Underline(PrinterModeState state);
        byte[] Expanded(string value);
        byte[] Expanded(PrinterModeState state);
        byte[] Condensed(string value);
        byte[] Condensed(PrinterModeState state);
        byte[] Font(string value, Fonts state);
        byte[] Font(Fonts state);
    }
}