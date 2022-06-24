using System.Diagnostics;
using System.Net.WebSockets;
using System.Text.Json;
using Websocket.Client;

namespace WebPrintManager
{
    internal class PrintManagerConnection : IDisposable
    {
        private readonly JsonSerializerOptions jsonSerializerSettings = new JsonSerializerOptions();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly WebSocketClientFactory webSocketClientFactory = new WebSocketClientFactory();

        private IWebsocketClient? client;

        public event ReceivedMessageEventHandler? ReceivedMessage;
        public event EventHandler<string>? WsClosed;

        public int MsgCount { get; private set; }

        public DateTime ReferenceDate { get; set; } = DateTime.Now;

        public string? ClientId { get; private set; }

        private int ToUnixTimestamp(DateTime value)
        {
            return (int)Math.Truncate(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }

        private Uri CreateAddress(string host, int port)
        {
            if (port != 80)
            {
                return new Uri($"ws://{host}:{port}");
            }
            else
            {
                return new Uri($"ws://{host}");
            }
        }

        private Task Send(string message)
        {
            if (this.client == null)
            {
                throw new InvalidOperationException("Socket is null");
            }

            return this.client.SendInstant(message);
        }

        private Task SendBinary(byte[] message)
        {
            if (this.client == null)
            {
                throw new InvalidOperationException("Socket is null");
            }

            return this.client.SendInstant(message);
        }

        public string GenerateMessageTag(bool longTag)
        {
            var seconds = this.ToUnixTimestamp(this.ReferenceDate);
            var value = longTag ? seconds : (seconds % 1000);
            var tag = $"{value}.--{this.MsgCount}";
            this.MsgCount++;
            return tag;
        }

        public async Task<string> SendJson(object? json, Messages.MessageType type, string? tag, bool longTag)
        {
            if (tag == null)
            {
                tag = this.GenerateMessageTag(longTag);
            }

            var jsonText = json != null ? JsonSerializer.Serialize(json, this.jsonSerializerSettings) : null;
            await this.Send($"{tag},{type.ToString().ToLowerInvariant()},{jsonText}");
            return tag;
        }

        public async Task<string> SendBinary(byte[] data, Messages.MessageType type, string? tag, bool longTag)
        {
            if (tag == null)
            {
                tag = this.GenerateMessageTag(longTag);
            }

            var header = System.Text.Encoding.UTF8.GetBytes($"{tag},{type.ToString().ToLowerInvariant()},");
            var message = new byte[header.Length + data.Length];

            Buffer.BlockCopy(header, 0, message, 0, header.Length);
            Buffer.BlockCopy(data, 0, message, header.Length, data.Length);

            await this.SendBinary(message);
            return tag;
        }

        public async Task<string?> WaitForMessage(
            string tag,
            int timeoutMs,
            CancellationToken cancellationToken)
        {
            ReceivedMessage? message = null;
            Exception? error = null;
            bool isCompleted = false;

            EventHandler<string>? onError = null;
            ReceivedMessageEventHandler? onRecv = null;

            onRecv = (e) =>
            {
                if (e.Tag == tag)
                {
                    this.ReceivedMessage -= onRecv;
                    this.WsClosed -= onError;
                    message = e;
                    isCompleted = true;
                    return true;
                }

                return false;
            };

            onError = (_, reason) =>
            {
                this.ReceivedMessage -= onRecv;
                this.WsClosed -= onError;
                error = new InvalidOperationException(reason);
                isCompleted = true;
            };

            this.ReceivedMessage += onRecv;
            this.WsClosed += onError;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (
                !cancellationToken.IsCancellationRequested &&
                !isCompleted &&
                stopWatch.ElapsedMilliseconds < timeoutMs)
            {
                await Task.Delay(50);
            }

            stopWatch.Stop();

            if (!isCompleted)
            {
                throw new InvalidOperationException("Timed out");
            }

            if (cancellationToken.IsCancellationRequested)
            {
                throw new InvalidOperationException("Cancelled");
            }

            if (error != null)
            {
                throw error;
            }
            else
            {
                return message?.Response;
            }
        }

        public async Task Connect(string host, int port)
        {
            var webSocketClient = this.webSocketClientFactory.Create(this.CreateAddress(host, port));
            webSocketClient.ErrorReconnectTimeout = TimeSpan.FromSeconds(5);
            webSocketClient.MessageReceived.Subscribe(this.OnWsMessageReceived);
            webSocketClient.DisconnectionHappened.Subscribe(this.OnWsDisconnect);
            webSocketClient.IsReconnectionEnabled = true;

            this.client = webSocketClient;
            try
            {
                await webSocketClient.Start();
            }
            catch
            {
                this.client = null;
                webSocketClient.Dispose();
            }
        }

        private (string MessageTag, Messages.MessageType Type, string Response) Decrypt(string message)
        {
            var messageTagIndex = message.IndexOf(",", StringComparison.Ordinal);
            if (messageTagIndex < 0)
            {
                throw new InvalidOperationException("invalid message");
            }

            if (message.Length > (messageTagIndex + 1) && message[messageTagIndex + 1] == ',')
            {
                messageTagIndex++;
            }

            var typeIndex = message.IndexOf(",", messageTagIndex + 1, StringComparison.Ordinal);
            if (typeIndex < 0)
            {
                throw new InvalidOperationException("invalid message");
            }

            if (message.Length > (typeIndex + 1) && message[typeIndex + 1] == ',')
            {
                typeIndex++;
            }

            var messageTag = message.Substring(0, messageTagIndex);
            Enum.TryParse<Messages.MessageType>(message.Substring(messageTagIndex + 1, typeIndex - messageTagIndex - 1), true, out var type);
            var data = message.Substring(typeIndex + 1);

            return (messageTag, type, data);
        }

        public void EndConnection(string reason)
        {
            if (this.client != null)
            {
                this.WsClosed?.Invoke(this, reason);
            }

            this.MsgCount = 0;
        }

        private void OnWsDisconnect(DisconnectionInfo info)
        {
            if (this.cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            this.EndConnection("lost");
        }

        private void OnWsMessageReceived(ResponseMessage responseMessage)
        {
            string? messageTag = null;
            var type = Messages.MessageType.Unknown;
            string? response = null;

            try
            {
                if (responseMessage.MessageType == WebSocketMessageType.Text)
                {
                    (messageTag, type, response) = this.Decrypt(responseMessage.Text);
                }
            }
            catch
            {
                // ignore
            }

            if (response == null)
            {
                return;
            }

            this.ReceivedMessage?.Invoke(new ReceivedMessage(messageTag, type, response));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.cancellationTokenSource != null)
            {
                if (!this.cancellationTokenSource.IsCancellationRequested)
                {
                    this.cancellationTokenSource.Cancel();
                }

                this.cancellationTokenSource.Dispose();
            }

            this.client?.Dispose();
            this.client = null;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
