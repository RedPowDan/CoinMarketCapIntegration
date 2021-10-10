namespace Domain.Services
{
    using System;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Hosting;

    public class CryptoUpdaterHostedService : BackgroundService
    {
        private readonly ILogger<CryptoUpdaterHostedService> _logger;
        private readonly ICryptoService _cryptoService;

        private readonly int _sleepForTaskInMinutes = 10;

        public CryptoUpdaterHostedService(
            ILogger<CryptoUpdaterHostedService> logger,
            ICryptoService cryptoService)
        {
            _logger = logger;
            _cryptoService = cryptoService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RunCryptoUpdater is running");

            while (true)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Stopping token cancelled requested");
                    return;
                }

                try
                {
                    _cryptoService.UpdateCryptoForAllCurrencies(stoppingToken);

                    _logger.LogInformation($"Sleep for {_sleepForTaskInMinutes} minutes...");
                    await Task
                        .Delay(TimeSpan.FromMinutes(_sleepForTaskInMinutes), stoppingToken)
                        .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Crypto update error");
                }
            }
        }

    }
}