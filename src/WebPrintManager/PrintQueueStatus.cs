namespace WebPrintManager
{
    [Flags]
    public enum PrintQueueStatus
    {
        None = 0,
        Paused = 1,
        Error = 2,
        PendingDeletion = 4,
        PaperJam = 8,
        PaperOut = 0x10,
        ManualFeed = 0x20,
        PaperProblem = 0x40,
        Offline = 0x80,
        IOActive = 0x100,
        Busy = 0x200,
        Printing = 0x400,
        OutputBinFull = 0x800,
        NotAvailable = 0x1000,
        Waiting = 0x2000,
        Processing = 0x4000,
        Initializing = 0x8000,
        WarmingUp = 0x10000,
        TonerLow = 0x20000,
        NoToner = 0x40000,
        PagePunt = 0x80000,
        UserIntervention = 0x100000,
        OutOfMemory = 0x200000,
        DoorOpen = 0x400000,
        ServerUnknown = 0x800000,
        PowerSave = 0x1000000,
    }
}
