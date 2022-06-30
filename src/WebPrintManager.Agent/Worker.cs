namespace WebPrintManager.Agent
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ManagerServer server;
        private readonly Printing.PrintMonitor printMonitor;

        public Worker(ManagerServer server, Printing.PrintMonitor printMonitor, ILogger<Worker> logger)
        {
            this.server = server;
            this.logger = logger;
            this.printMonitor = printMonitor;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"Start over port {this.server.Port}");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            this.server.Stop();
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (this.server != null)
            {
                this.server.Start();
            }

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.server.Dispose();
            this.printMonitor.Dispose();
        }
    }
}
