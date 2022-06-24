using Microsoft.Extensions.Options;
using NetCoreServer;
using System.Net;

namespace WebPrintManager.Agent
{
    internal class ManagerServer : WsServer
    {
        public ManagerServer(IOptionsMonitor<ManagerServerOptions> options)
            : base(
                  IPAddress.TryParse(options.CurrentValue.Address, out var ipAddress) ? ipAddress : IPAddress.Any,
                  options.CurrentValue.Port)
        {
        }

        protected override TcpSession CreateSession()
        {
            var session = new ManagerSession(this);
            session.Configure(Messages.MessageType.PrintersList, new Services.PrintersService(session));
            session.Configure(Messages.MessageType.RawPrint, new Services.RawPrintService(session));

            return session;
        }
    }
}
