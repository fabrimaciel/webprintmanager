using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DocInfoA
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
