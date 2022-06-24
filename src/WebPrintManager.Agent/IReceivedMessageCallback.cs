namespace WebPrintManager.Agent
{
    internal interface IReceivedMessageCallback
    {
        void Execute(string tag, byte[] buffer, int offset, int size);
    }
}