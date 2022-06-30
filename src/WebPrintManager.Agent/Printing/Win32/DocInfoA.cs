using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class DocInfoA
    {
#pragma warning disable
        [MarshalAs(UnmanagedType.LPStr)]
        public string pDocName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pOutputFile;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pDataType;
#pragma warning restore
    }
}
