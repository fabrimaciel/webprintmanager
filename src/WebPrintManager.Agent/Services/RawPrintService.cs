using System.Text;

namespace WebPrintManager.Agent.Services
{
    internal class RawPrintService : IReceivedMessageCallback
    {
        private readonly ManagerSession session;

        public RawPrintService(ManagerSession session)
        {
            this.session = session;
        }

        public void Execute(string tag, byte[] buffer, int offset, int size)
        {
            var enconding = Encoding.UTF8;
            var printName = new StringBuilder();
            var index = -1;

            try
            {
                for (var i = offset; i < (offset + size); i++)
                {
                    if (buffer[i] == ',')
                    {
                        index = i - offset + 1;
                        break;
                    }
                    else
                    {
                        printName.Append(enconding.GetString(buffer, i, 1));
                    }
                }

                if (index > 0)
                {
                    var body = new byte[size - index];
                    Buffer.BlockCopy(buffer, index + offset, body, 0, body.Length);
                    Printing.RawPrinter.SendBytesToPrinter(printName.ToString(), body);
                }
            }
            catch (Exception ex)
            {
                this.session.SendJson(new Messages.RawPrintResult(false, ex.Message), "message", tag, false);
                return;
            }

            this.session.SendJson(new Messages.RawPrintResult(true, null), "message", tag, false);
        }
    }
}
