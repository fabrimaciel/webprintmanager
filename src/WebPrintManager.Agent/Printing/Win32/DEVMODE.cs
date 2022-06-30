using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;

        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;

        public short dmLogPixels;
        public short dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;

        public override string ToString()
        {
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                @"dmDeviceName == '{0}',
dmSpecVersion == {1},
dmDriverVersion == {2},
dmSize == {3},
dmDriverExtra == {4},
dmFields == {5},
dmPositionX == {6},
dmPositionY == {7},
dmDisplayOrientation == {8},
dmDisplayFixedOutput == {9},
dmColor == {10},
dmDuplex == {11},
dmYResolution == {12},
dmTTOption == {13},
dmCollate == {14},
dmFormName == {15},
dmLogPixels == {16},
dmBitsPerPel == {17},
dmPelsWidth == {18},
dmPelsHeight == {19},
dmDisplayFlags == {20},
dmDisplayFrequency == {21},
dmICMMethod == {22},
dmICMIntent == {23},
dmMediaType == {24},
dmPanningWidth == {25},
dmPanningHeight == {26}",
                dmDeviceName,
                dmSpecVersion,
                dmDriverVersion,
                dmSize,
                dmDriverExtra,
                dmFields,
                dmPositionX,
                dmPositionY,
                dmDisplayOrientation,
                dmDisplayFixedOutput,
                dmColor,
                dmDuplex,
                dmYResolution,
                dmTTOption,
                dmCollate,
                dmFormName,
                dmLogPixels,
                dmBitsPerPel,
                dmPelsWidth,
                dmPelsHeight,
                dmDisplayFlags,
                dmDisplayFrequency,
                dmICMMethod,
                dmICMIntent,
                dmMediaType,
                dmPanningWidth,
                dmPanningHeight);
        }
    }
#pragma warning enable
}
