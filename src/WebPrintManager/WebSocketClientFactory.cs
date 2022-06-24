using Websocket.Client;

namespace WebPrintManager
{
    internal class WebSocketClientFactory
    {
        public WebsocketClient Create(Uri address)
        {
            return new WebsocketClient(
                address,
                async (uri, cancellationToken) =>
                {
                    var ws = new System.Net.WebSockets.ClientWebSocket();
                    await ws.ConnectAsync(uri, cancellationToken);
                    return ws;
                });
        }
    }
}
