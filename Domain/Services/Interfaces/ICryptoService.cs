namespace Domain.Services.Interfaces
{
    using System.Threading;
    using Models;

    /// <summary>
    /// Интерфейс с криптовалютой. Загрузка, создание, обновление.
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// Возвращает количество моделей в таблице crypto
        /// </summary>
        /// <returns></returns>
        public int GetCountCryptoModelsInDb();

        /// <summary>
        /// Получает айдишники моделей в API 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public int[] GetIdsInApi(Crypto[] models);

        /// <summary>
        /// Обновляет, либо создает модели
        /// </summary>
        /// <param name="models"></param>
        public void UpdateOrCreateCrypto(Crypto[] models);

        /// <summary>
        /// Используется для загрузки, создания, обновления информации про криптовалюту
        /// </summary>
        /// <param name="stoppingToken"></param>
        public void UpdateCryptoForAllCurrencies(CancellationToken stoppingToken);

        /// <summary>
        /// Получает модели crypto с фильтрами
        /// </summary>
        /// <param name="searchString">Искомая строка</param>
        /// <param name="sortOrder">Сортировка</param>
        /// <returns></returns>
        public Crypto[] GetCryptoModelsWithFilters(
            string searchString,
            string sortOrder);

        /// <summary>
        /// Мапит и выводит то кол-во которое задано
        /// </summary>
        /// <param name="cryptoModels"></param>
        /// <param name="countOutput"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public Crypto[] GetCryptoInfosWithPaginate(
            Crypto[] cryptoModels,
            int countOutput,
            int pageNumber);
    }
}