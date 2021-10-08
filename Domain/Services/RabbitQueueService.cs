using System.Collections.Generic;
using Domain.Dtos;

namespace Domain.Services
{
    using Interfaces;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    class RabbitQueueService : IQueueService, IDisposable
    {
        private const string CryptoForecastRoutingKey = "crypto_forecast";
        private const string ExchangeName = "CryptoMarketCapIntegration";

        private readonly Dictionary<Type, string> _dtoTypeToRoutingKeyMap =
            new Dictionary<Type, string>
            {
                {typeof(CryptoDto), CryptoForecastRoutingKey}
            };

        private readonly ILogger<RabbitQueueService> _logger;

        public Task Push<T>(CancellationToken stoppingToken, T message)
        {
            throw new NotImplementedException();
        }

        public Task Subscribe<T>(CancellationToken stoppingToken, Func<T, CancellationToken, Task> handler) where T : class
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}