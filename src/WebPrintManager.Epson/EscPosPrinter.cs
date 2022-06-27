using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebPrintManager.Epson.Commands;

namespace WebPrintManager.Epson
{
    public class EscPosPrinter : IEscPosPrinter
    {
        private readonly Encoding encoding;
        private readonly IPrintCommand command;
        private byte[] buffer;

        public EscPosPrinter(Encoding encoding)
        {
            this.encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            this.command = new EscPos();
        }

        public int ColsNomal
        {
            get
            {
                return this.command.ColsNomal;
            }
        }

        public int ColsCondensed
        {
            get
            {
                return this.command.ColsCondensed;
            }
        }

        public int ColsExpanded
        {
            get
            {
                return this.command.ColsExpanded;
            }
        }

        public Task PrintDocument(System.IO.Stream outputStream, CancellationToken cancellationToken)
        {
            if (outputStream is null)
            {
                throw new ArgumentNullException(nameof(outputStream));
            }

            if (this.buffer == null)
            {
                return Task.CompletedTask;
            }

            return outputStream.WriteAsync(this.buffer, 0, this.buffer.Length, cancellationToken);
        }

        public void Append(string value)
        {
            this.AppendString(value, true);
        }

        public void Append(byte[] value)
        {
            if (value == null)
            {
                return;
            }

            var list = new List<byte>();
            if (this.buffer != null)
            {
                list.AddRange(this.buffer);
            }

            list.AddRange(value);
            this.buffer = list.ToArray();
        }

        public void AppendWithoutLf(string value)
        {
            this.AppendString(value, false);
        }

        private void AppendString(string value, bool useLf)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (useLf)
            {
                value += "\n";
            }

            var list = new List<byte>();
            if (this.buffer != null)
            {
                list.AddRange(this.buffer);
            }

            var bytes = this.encoding.GetBytes(value);
            list.AddRange(bytes);
            this.buffer = list.ToArray();
        }

        public void NewLine()
        {
            this.Append("\r");
        }

        public void NewLines(int lines)
        {
            for (int i = 1, loopTo = lines - 1; i <= loopTo; i++)
            {
                this.NewLine();
            }
        }

        public void Clear()
        {
            this.buffer = null;
        }

        public void Separator(char speratorChar = '-')
        {
            this.Append(this.command.Separator(speratorChar));
        }

        public void AutoTest()
        {
            this.Append(this.command.AutoTest());
        }

        public async Task TestPrinter(System.IO.Stream outputStream, CancellationToken cancellationToken)
        {
            if (outputStream is null)
            {
                throw new ArgumentNullException(nameof(outputStream));
            }

            this.Append("NORMAL - 48 COLUMNS");
            this.Append("1...5...10...15...20...25...30...35...40...45.48");
            this.Separator();
            this.Append("Text Normal");
            this.BoldMode("Bold Text");
            this.UnderlineMode("Underlined text");
            this.Separator();
            this.ExpandedMode(PrinterModeState.On);
            this.Append("Expanded - 23 COLUMNS");
            this.Append("1...5...10...15...20..23");
            this.ExpandedMode(PrinterModeState.Off);
            this.Separator();
            this.CondensedMode(PrinterModeState.On);
            this.Append("Condensed - 64 COLUMNS");
            this.Append("1...5...10...15...20...25...30...35...40...45...50...55...60..64");
            this.CondensedMode(PrinterModeState.Off);
            this.Separator();
            this.DoubleWidth2();
            this.Append("Font Width 2");
            this.DoubleWidth3();
            this.Append("Font Width 3");
            this.NormalWidth();
            this.Append("Normal width");
            this.Separator();
            this.AlignRight();
            this.Append("Right aligned text");
            this.AlignCenter();
            this.Append("Center-aligned text");
            this.AlignLeft();
            this.Append("Left aligned text");
            this.Separator();
            this.Font("Font A", Fonts.FontA);
            this.Font("Font B", Fonts.FontB);
            this.Font("Font C", Fonts.FontC);
            this.Font("Font D", Fonts.FontD);
            this.Font("Font E", Fonts.FontE);
            this.Font("Font Special A", Fonts.SpecialFontA);
            this.Font("Font Special B", Fonts.SpecialFontB);
            this.Separator();
            await outputStream.WriteAsync(this.buffer, 0, this.buffer.Length, cancellationToken);
            await this.InitializePrint(outputStream, cancellationToken);
            this.Clear();
            this.SetLineHeight(24);
            this.Append("This is first line with line height of 30 dots");
            this.SetLineHeight(40);
            this.Append("This is second line with line height of 24 dots");
            this.Append("This is third line with line height of 40 dots");
            this.NewLines(3);
            this.Append("End of Test :)");
            this.Separator();
            await outputStream.WriteAsync(this.buffer, 0, this.buffer.Length, cancellationToken);
        }

        public void BoldMode(string value)
        {
            this.Append(this.command.FontMode.Bold(value));
        }

        public void BoldMode(PrinterModeState state)
        {
            this.Append(this.command.FontMode.Bold(state));
        }

        public void Font(string value, Fonts state)
        {
            this.Append(this.command.FontMode.Font(value, state));
        }

        public void UnderlineMode(string value)
        {
            this.Append(this.command.FontMode.Underline(value));
        }

        public void UnderlineMode(PrinterModeState state)
        {
            this.Append(this.command.FontMode.Underline(state));
        }

        public void ExpandedMode(string value)
        {
            this.Append(this.command.FontMode.Expanded(value));
        }

        public void ExpandedMode(PrinterModeState state)
        {
            this.Append(this.command.FontMode.Expanded(state));
        }

        public void CondensedMode(string value)
        {
            this.Append(this.command.FontMode.Condensed(value));
        }

        public void CondensedMode(PrinterModeState state)
        {
            this.Append(this.command.FontMode.Condensed(state));
        }

        public void NormalWidth()
        {
            this.Append(this.command.FontWidth.Normal());
        }

        public void DoubleWidth2()
        {
            this.Append(this.command.FontWidth.DoubleWidth2());
        }

        public void DoubleWidth3()
        {
            this.Append(this.command.FontWidth.DoubleWidth3());
        }

        public void AlignLeft()
        {
            this.Append(this.command.Alignment.Left());
        }

        public void AlignRight()
        {
            this.Append(this.command.Alignment.Right());
        }

        public void AlignCenter()
        {
            this.Append(this.command.Alignment.Center());
        }

        public void FullPaperCut()
        {
            this.Append(this.command.PaperCut.Full());
        }

        public void PartialPaperCut()
        {
            this.Append(this.command.PaperCut.Partial());
        }

        public void OpenDrawer()
        {
            this.Append(this.command.Drawer.Open());
        }

        public void QrCode(string qrData)
        {
            this.Append(this.command.QrCode.Print(qrData));
        }

        public void QrCode(string qrData, QrCodeSize qrCodeSize)
        {
            this.Append(this.command.QrCode.Print(qrData, qrCodeSize));
        }

        public void Code128(string code, Positions positions)
        {
            this.Append(this.command.BarCode.Code128(code, positions));
        }

        public void Code39(string code, Positions positions)
        {
            this.Append(this.command.BarCode.Code39(code, positions));
        }

        public void Ean13(string code, Positions positions)
        {
            this.Append(this.command.BarCode.Ean13(code, positions));
        }

        public Task InitializePrint(System.IO.Stream outputStream, CancellationToken cancellationToken)
        {
            if (outputStream is null)
            {
                throw new ArgumentNullException(nameof(outputStream));
            }

            var content = this.command.InitializePrint.Initialize();
            return outputStream.WriteAsync(content, 0, content.Length, cancellationToken);
        }

        public void Image(Bitmap image)
        {
            this.Append(this.command.Image.Print(image));
        }

        public void NormalLineHeight()
        {
            this.Append(this.command.LineHeight.Normal());
        }

        public void SetLineHeight(byte height)
        {
            this.Append(this.command.LineHeight.SetLineHeight(height));
        }
    }
}