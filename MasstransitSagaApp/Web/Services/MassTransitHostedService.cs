using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Services
{
    public class MassTransitHostedService : IHostedService
    {
        private readonly IBusControl busControl;
        private readonly ILogger logger;

        public MassTransitHostedService(IBusControl busControl, ILogger logger)
        {
            this.busControl = busControl;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Masstransit Esb started");
            await busControl.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Masstransit Esb stopped");
            await busControl.StopAsync(cancellationToken);
        }
    }
}
