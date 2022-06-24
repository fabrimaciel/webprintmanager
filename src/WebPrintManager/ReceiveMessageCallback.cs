using System.Text.Json;

namespace WebPrintManager
{
    internal sealed class ReceiveMessageCallback<T> : IReceiveMessageCallback
    {
        private readonly Action<T> callback;

        public ReceiveMessageCallback(Action<T> callback)
        {
            this.callback = callback;
        }

        public void Execute(ReceivedMessage receivedMessage)
        {
            var message = (T)JsonSerializer.Deserialize(receivedMessage.Response, typeof(T)) !;

            Task.Run(() => this.callback.Invoke(message));
        }
    }
}
