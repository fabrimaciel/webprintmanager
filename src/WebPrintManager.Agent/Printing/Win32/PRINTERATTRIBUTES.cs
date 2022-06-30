namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [Flags]
    internal enum PRINTERATTRIBUTES
    {
        QUEUED = 1,
        DIRECT = 2,
        DEFAULT = 4,
        SHARED = 8,
        NETWORK = 0x10,
        HIDDEN = 0x20,
        LOCAL = 0x40,
        ENABLE_DEVQ = 0x80,
        KEEPPRINTEDJOBS = 0x100,
        DO_COMPLETE_FIRST = 0x200,
        WORK_OFFLINE = 0x400,
        ENABLE_BIDI = 0x800,
        RAW_ONLY = 0x1000,
        PUBLISHED = 0x2000,
    }
#pragma warning enable
}
