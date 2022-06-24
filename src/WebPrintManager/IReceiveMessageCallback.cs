namespace WebPrintManager
{
    internal interface IReceiveMessageCallback
    {
        void Execute(ReceivedMessage receivedMessage);
    }
}
