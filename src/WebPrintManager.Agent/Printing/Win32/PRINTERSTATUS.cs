﻿namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [Flags]
    internal enum PRINTERSTATUS
    {
        PRINTER_STATUS_PAUSED = 1,
        PRINTER_STATUS_ERROR = 2,
        PRINTER_STATUS_PENDING_DELETION = 4,
        PRINTER_STATUS_PAPER_JAM = 8,
        PRINTER_STATUS_PAPER_OUT = 0x10,
        PRINTER_STATUS_MANUAL_FEED = 0x20,
        PRINTER_STATUS_PAPER_PROBLEM = 0x40,
        PRINTER_STATUS_OFFLINE = 0x80,
        PRINTER_STATUS_IO_ACTIVE = 0x100,
        PRINTER_STATUS_BUSY = 0x200,
        PRINTER_STATUS_PRINTING = 0x400,
        PRINTER_STATUS_OUTPUT_BIN_FULL = 0x800,
        PRINTER_STATUS_NOT_AVAILABLE = 0x1000,
        PRINTER_STATUS_WAITING = 0x2000,
        PRINTER_STATUS_PROCESSING = 0x4000,
        PRINTER_STATUS_INITIALIZING = 0x8000,
        PRINTER_STATUS_WARMING_UP = 0x10000,
        PRINTER_STATUS_TONER_LOW = 0x20000,
        PRINTER_STATUS_NO_TONER = 0x40000,
        PRINTER_STATUS_PAGE_PUNT = 0x80000,
        PRINTER_STATUS_USER_INTERVENTION = 0x100000,
        PRINTER_STATUS_OUT_OF_MEMORY = 0x200000,
        PRINTER_STATUS_DOOR_OPEN = 0x400000,
        PRINTER_STATUS_SERVER_UNKNOWN = 0x800000,
        PRINTER_STATUS_POWER_SAVE = 0x1000000,
    }
#pragma warning enable
}
