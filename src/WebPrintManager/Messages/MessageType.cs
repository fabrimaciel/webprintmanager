﻿namespace WebPrintManager.Messages
{
    public enum MessageType
    {
        Unknown = 0,
        Connection,
        PrintersList,
        RawPrint,
        PrintJobStatus,
        GetPrintJobInfo,
        CancelJob,
        PrinterInfo,
        PrinterPurge,
    }
}
