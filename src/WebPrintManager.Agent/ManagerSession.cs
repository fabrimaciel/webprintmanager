﻿using NetCoreServer;
using System.Text.Json;

namespace WebPrintManager.Agent
{
    internal class ManagerSession : WsSession
    {
        private readonly JsonSerializerOptions jsonSerializerSettings = new JsonSerializerOptions();
        private readonly IDictionary<Messages.MessageType, IReceivedMessageCallback> callbacks = new Dictionary<Messages.MessageType, IReceivedMessageCallback>();
        private readonly ManagerServerOptions options;

        public ManagerSession(WsServer server, ManagerServerOptions options)
            : base(server)
        {
            this.options = options;
        }

        private string? GetHeaderOrigin(HttpRequest request)
        {
            for (var i = 0; i < request.Headers; i++)
            {
                (var headerName, var headerValue) = request.Header(i);

                if (StringComparer.InvariantCultureIgnoreCase.Equals(headerName, "Origin"))
                {
                    return headerValue;
                }
            }

            return null;
        }

        protected override void OnReceivedRequest(HttpRequest request)
        {
            if (this.options.AuthorizedSites != null && this.options.AuthorizedSites.Any())
            {
                var origin = this.GetHeaderOrigin(request);

                if (!this.options.AuthorizedSites.Contains(origin, StringComparer.InvariantCultureIgnoreCase))
                {
                    this.Disconnect();
                }
            }

            base.OnReceivedRequest(request);
        }

        public DateTime ReferenceDate { get; set; } = DateTime.Now;

        public int MsgCount { get; private set; }

        private int ToUnixTimestamp(DateTime value)
        {
            return (int)Math.Truncate(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }

        public string GenerateMessageTag(bool longTag)
        {
            var seconds = this.ToUnixTimestamp(this.ReferenceDate);
            var value = longTag ? seconds : (seconds % 1000);
            var tag = $"{value}.--{this.MsgCount}";
            this.MsgCount++;
            return tag;
        }

        public string? SendJson(object? json, string type, string? tag, bool longTag)
        {
            if (tag == null)
            {
                tag = this.GenerateMessageTag(longTag);
            }

            var jsonText = json != null ? JsonSerializer.Serialize(json, this.jsonSerializerSettings) : null;
            if (this.SendTextAsync($"{tag},{type},{jsonText}"))
            {
                return tag;
            }

            return null;
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            this.SendJson(new Messages.ConnectionMessage(), "connection", null, false);
        }

        public void Configure(Messages.MessageType messageType, IReceivedMessageCallback callback)
        {
            this.callbacks.Add(messageType, callback);
        }

        public override void OnWsReceived(byte[] buffer, long offset, long size)
        {
            base.OnWsReceived(buffer, offset, size);

            long messageTagIndex = -1;
            long messageTypeIndex = -1;

            for (var i = offset; i < (offset + size) && (messageTagIndex < 0 || messageTypeIndex < 0); i++)
            {
                if (buffer[i] == ',')
                {
                    if (messageTagIndex < 0)
                    {
                        messageTagIndex = i;
                    }
                    else if (messageTypeIndex < 0)
                    {
                        messageTypeIndex = i;
                    }
                }
            }

            if (messageTagIndex >= 0 && messageTypeIndex >= 0)
            {
                var encoding = System.Text.Encoding.UTF8;

                var tag = encoding.GetString(buffer, (int)offset, (int)messageTagIndex);
                var type = encoding.GetString(buffer, (int)messageTagIndex + 1, (int)(messageTypeIndex - messageTagIndex) - 1);

                if (Enum.TryParse<Messages.MessageType>(type, true, out var messageType))
                {
                    this.OnReceived(tag, messageType, buffer, messageTypeIndex + 1, size - (messageTypeIndex + 1));
                }
            }
        }

        private void OnReceived(string tag, Messages.MessageType type, byte[] buffer, long offset, long size)
        {
            if (this.callbacks.TryGetValue(type, out var callback))
            {
                callback.Execute(tag, buffer, (int)offset, (int)size);
            }
        }

        protected override void Dispose(bool disposingManagedResources)
        {
            base.Dispose(disposingManagedResources);

            foreach (var callback in this.callbacks.Select(f => f.Value).OfType<IDisposable>())
            {
                callback.Dispose();
            }
        }
    }
}
