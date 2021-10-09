using System.Threading;
using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface ICryptoService
    {
        public int[] GetIdsInApi(Crypto[] models);

        public void UpdateOrCreateCrypto(Crypto[] models);

        public void UpdateCryptoForAllCurrencies(CancellationToken stoppingToken);
    }
}
