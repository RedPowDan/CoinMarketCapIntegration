namespace Domain.Services.Interfaces
{
    using System.Threading;
    using Dtos.Crypto;
    using Models;

    public interface ICryptoService
    {
        public int[] GetIdsInApi(Crypto[] models);

        public void UpdateOrCreateCrypto(Crypto[] models);

        public void UpdateCryptoForAllCurrencies(CancellationToken stoppingToken);

        public CryptoInfoDto[] GetCryptoInfosWithFilters(
            string searchString,
            string sortOrder,
            int countOutput,
            int pageNumber);
    }
}