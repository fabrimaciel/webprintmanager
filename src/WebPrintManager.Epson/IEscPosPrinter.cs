using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace WebPrintManager.Epson
{
    public interface IEscPosPrinter
    {
        int ColsNomal { get; }

        int ColsCondensed { get; }

        int ColsExpanded { get; }

        Task PrintDocument(System.IO.Stream outputStream, CancellationToken cancellationToken);

        void Append(string value);

        void Append(byte[] value);

        void AppendWithoutLf(string value);

        void NewLine();

        void NewLines(int lines);

        void Clear();

        void Separator(char speratorChar = '-');

        void AutoTest();

        Task TestPrinter(System.IO.Stream outputStream, CancellationToken cancellationToken);

        void Font(string value, Fonts state);

        void BoldMode(string value);

        void BoldMode(PrinterModeState state);

        void UnderlineMode(string value);

        void UnderlineMode(PrinterModeState state);

        void ExpandedMode(string value);

        void ExpandedMode(PrinterModeState state);

        void CondensedMode(string value);

        void CondensedMode(PrinterModeState state);

        void NormalWidth();

        void DoubleWidth2();

        void DoubleWidth3();

        void NormalLineHeight();

        void SetLineHeight(byte height);

        void AlignLeft();

        void AlignRight();

        void AlignCenter();

        void FullPaperCut();

        void PartialPaperCut();

        void OpenDrawer();

        void Image(Bitmap image);

        void QrCode(string qrData);

        void QrCode(string qrData, QrCodeSize qrCodeSize);

        void Code128(string text, Positions positions);

        void Code128(string text, Positions positions, BarWidth width, int height, bool useFontB, BarcodeCode code);

        void Code39(string text, Positions positions);

        void Code39(string text, Positions positions, BarWidth width, int height, bool useFontB);

        void Ean13(string text, Positions positions);

        void Ean13(string text, Positions positions, BarWidth width, int height, bool useFontB);

        Task InitializePrint(System.IO.Stream outputStream, CancellationToken cancellationToken);
    }
}
