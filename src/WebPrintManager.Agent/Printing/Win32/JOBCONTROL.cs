namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    internal enum JOBCONTROL
    {
        JOB_CONTROL_PAUSE = 1,
        JOB_CONTROL_RESUME = 2,
        JOB_CONTROL_CANCEL = 3,
        JOB_CONTROL_RESTART = 4,
        JOB_CONTROL_DELETE = 5,
        JOB_CONTROL_SENT_TO_PRINTER = 6,
        JOB_CONTROL_LAST_PAGE_EJECTED = 7,
        JOB_CONTROL_RETAIN = 8,
        JOB_CONTROL_RELEASE = 9,
    }
#pragma warning enable
}
