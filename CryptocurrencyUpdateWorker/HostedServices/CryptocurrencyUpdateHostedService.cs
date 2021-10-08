namespace CryptocurrencyUpdateWorker.HostedServices
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class CryptocurrencyUpdateHostedService : BackgroundService
    {
        private readonly ILogger<CryptocurrencyUpdateHostedService> _logger;
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
