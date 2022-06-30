namespace WebPrintManager
{
    [Flags]
    public enum PrintJobStatus
    {
        None = 0,
        Paused = 1,
        Error = 2,
        Deleting = 4,
        Spooling = 8,
        Printing = 0x10,
        Offline = 0x20,
        PaperOut = 0x40,
        Printed = 0x80,
        Deleted = 0x100,
        Blocked = 0x200,
        UserIntervention = 0x400,
        Restarted = 0x800,
        Completed = 0x1000,
        Retained = 0x2000,
    }
}
