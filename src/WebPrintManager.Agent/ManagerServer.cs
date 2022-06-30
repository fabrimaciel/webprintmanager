using Microsoft.Extensions.Options;
using NetCoreServer;
using System.Net;

namespace WebPrintManager.Agent
{
    internal class ManagerServer : WsServer
    {
        private readonly Printing.PrintMonitor printMonitor;
        private readonly Printing.ClientPrinterFactory clientPrinterFactory;
        private readonly IOptionsMonitor<ManagerServerOptions> options;

        public ManagerServer(
            Printing.PrintMonitor printMonitor,
            Printing.ClientPrinterFactory clientPrinterFactory,
            IOptionsMonitor<ManagerServerOptions> options)
            : base(
                  IPAddress.TryParse(options.CurrentValue.Address, out var ipAddress) ? ipAddress : IPAddress.Any,
                  options.CurrentValue.Port)
        {
            this.printMonitor = printMonitor;
            this.clientPrinterFactory = clientPrinterFactory;
            this.options = options;
        }

        protected override TcpSession CreateSession()
        {
            var session = new ManagerSession(this, this.options.CurrentValue);
            session.Configure(Messages.MessageType.PrintersList, new Services.PrintersService(session));
            session.Configure(Messages.MessageType.RawPrint, new Services.RawPrintService(session, this.printMonitor, this.clientPrinterFactory));
            session.Configure(Messages.MessageType.GetPrintJobInfo, new Services.GetJobInfoService(session, this.printMonitor, this.clientPrinterFactory));
            session.Configure(Messages.MessageType.CancelJob, new Services.CancelJobService(session, this.printMonitor, this.clientPrinterFactory));
            session.Configure(Messages.MessageType.PrinterInfo, new Services.GetPrinterInfoService(session, this.printMonitor, this.clientPrinterFactory));
            session.Configure(Messages.MessageType.PrinterPurge, new Services.PrinterPurgeService(session, this.printMonitor, this.clientPrinterFactory));

            return session;
        }
    }
}
