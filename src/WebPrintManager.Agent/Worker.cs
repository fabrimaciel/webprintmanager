using Microsoft.Extensions.Options;

namespace WebPrintManager.Agent
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ManagerServer server;

        public Worker(ManagerServer server, ILogger<Worker> logger)
        {
            this.server = server;
            this.logger = logger;
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
            this.server?.Dispose();
        }
    }
}
