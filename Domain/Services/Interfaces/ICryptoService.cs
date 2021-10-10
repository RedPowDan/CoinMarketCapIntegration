namespace Domain.Services.Interfaces
{
    using System.Threading;
    using Dtos.Crypto;
    using Models;

    public interface ICryptoService
    {
        public int GetCountCryptoModelsInDb();

        public int[] GetIdsInApi(Crypto[] models);

        public void UpdateOrCreateCrypto(Crypto[] models);

        public void UpdateCryptoForAllCurrencies(CancellationToken stoppingToken);

        public Crypto[] GetCryptoModelsWithFilters(
            string searchString,
            string sortOrder);

        public Crypto[] GetCryptoInfosWithPaginate(
            Crypto[] cryptoModels,
            int countOutput,
            int pageNumber);
    }
}