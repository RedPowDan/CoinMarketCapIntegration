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
        public void UpdateCryptoForAllCurrencies();

        /// <summary>
        /// Получает модели crypto с фильтрами
        /// </summary>
        /// <param name="searchString">Искомая строка</param>
        /// <param name="sortOrder">Сортировка</param>
        /// <param name="countOutput">Количество выводимых моделей</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <returns></returns>
        public Crypto[] GetCryptoModelsWithFiltersAndPaginate(
            string searchString,
            string sortOrder,
            int countOutput, 
            int pageNumber);
    }
}