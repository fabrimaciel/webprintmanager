namespace WebPrintManager.Agent.Printing
{
    internal interface IRawPrinter
    {
        PrintJobInfo Write(
            IClientPrinter printer,
            PrintDocumentInfo documentInfo,
            byte[] data,
            int offset,
            int count);
    }
}
