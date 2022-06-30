using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPrintManager.Agent.Services
{
    internal class GetPrinterInfoService : IReceivedMessageCallback
    {
        private readonly ManagerSession session;
        private readonly Printing.PrintMonitor printMonitor;
        private readonly Printing.ClientPrinterFactory clientPrinterFactory;

        public GetPrinterInfoService(
            ManagerSession session,
            Printing.PrintMonitor printMonitor,
            Printing.ClientPrinterFactory clientPrinterFactory)
        {
            this.session = session;
            this.printMonitor = printMonitor;
            this.clientPrinterFactory = clientPrinterFactory;
        }

        public void Execute(string tag, byte[] buffer, int offset, int size)
        {
            Messages.GetPrinterInfoResult? result = null;

            using (var inputStream = new MemoryStream(buffer, offset, size))
            {
                var message = System.Text.Json.JsonSerializer.Deserialize<Messages.PrintJobMessage>(inputStream) !;
                var printer = this.clientPrinterFactory.Create(message.PrinterType, message.PrinterData);

                try
                {
                    if (printer is Printing.ISpoolerPrinter spoolerPrinter)
                    {
                        var queue = this.printMonitor.Configure(spoolerPrinter.SpoolerName);

                        if (queue != null)
                        {
                            var info = queue.GetPrinterInfo();

                            if (info.HasValue)
                            {
                                result = new Messages.GetPrinterInfoResult((Messages.PrintQueueStatus)(int)info.Value.Status);
                            }
                        }
                    }
                }
                catch
                {
                    // ignore
                }
            }

            this.session.SendJson(result, "message", tag, false);
        }
    }
}
