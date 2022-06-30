namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    internal class PRINTER_CHANGES
    {
        public const uint PRINTER_CHANGE_ADD_PRINTER = 1;
        public const uint PRINTER_CHANGE_SET_PRINTER = 2;
        public const uint PRINTER_CHANGE_DELETE_PRINTER = 4;
        public const uint PRINTER_CHANGE_FAILED_CONNECTION_PRINTER = 8;
        public const uint PRINTER_CHANGE_PRINTER = 0xFF;
        public const uint PRINTER_CHANGE_ADD_JOB = 0x100;
        public const uint PRINTER_CHANGE_SET_JOB = 0x200;
        public const uint PRINTER_CHANGE_DELETE_JOB = 0x400;
        public const uint PRINTER_CHANGE_WRITE_JOB = 0x800;
        public const uint PRINTER_CHANGE_JOB = 0xFF00;
        public const uint PRINTER_CHANGE_ADD_FORM = 0x10000;
        public const uint PRINTER_CHANGE_SET_FORM = 0x20000;
        public const uint PRINTER_CHANGE_DELETE_FORM = 0x40000;
        public const uint PRINTER_CHANGE_FORM = 0x70000;
        public const uint PRINTER_CHANGE_ADD_PORT = 0x100000;
        public const uint PRINTER_CHANGE_CONFIGURE_PORT = 0x200000;
        public const uint PRINTER_CHANGE_DELETE_PORT = 0x400000;
        public const uint PRINTER_CHANGE_PORT = 0x700000;
        public const uint PRINTER_CHANGE_ADD_PRINT_PROCESSOR = 0x1000000;
        public const uint PRINTER_CHANGE_DELETE_PRINT_PROCESSOR = 0x4000000;
        public const uint PRINTER_CHANGE_PRINT_PROCESSOR = 0x7000000;
        public const uint PRINTER_CHANGE_ADD_PRINTER_DRIVER = 0x10000000;
        public const uint PRINTER_CHANGE_SET_PRINTER_DRIVER = 0x20000000;
        public const uint PRINTER_CHANGE_DELETE_PRINTER_DRIVER = 0x40000000;
        public const uint PRINTER_CHANGE_PRINTER_DRIVER = 0x70000000;
        public const uint PRINTER_CHANGE_TIMEOUT = 0x80000000;
        public const uint PRINTER_CHANGE_ALL = 0x7777FFFF;
    }
#pragma warning enable
}
