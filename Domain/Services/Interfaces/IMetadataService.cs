namespace Domain.Services.Interfaces
{
    using Dtos.Crypto;
    using Models;

    /// <summary>
    /// Интерфейс метаданных
    /// </summary>
    public interface IMetadataService
    {
        /// <summary>
        /// Выводит массив моделей по айдишникам
        /// </summary>
        /// <param name="Ids">Айдишники моделей</param>
        /// <returns></returns>
        public Metadata[] GetMetadataForIds(int[] Ids);

        /// <summary>
        /// Обновляет или создает модели
        /// </summary>
        /// <param name="newMetadataModels">Новые модели метаданных</param>
        public void UpdateOrCreateMetadataForModels(Metadata[] newMetadataModels);

        /// <summary>
        /// Получает айдишники моделей
        /// </summary>
        /// <param name="models">Модели</param>
        /// <returns></returns>
        public int[] GetIds(Metadata[] models);

        /// <summary>
        /// Получает айдишники модели crypto 
        /// </summary>
        /// <param name="models">Модели</param>
        /// <returns></returns>
        public int[] GetCryptoIds(Metadata[] models);

        /// <summary>
        /// Выводит новую мдель метаданных
        /// </summary>
        /// <param name="idCryptoModel">Айди модели crypto</param>
        /// <param name="metadata">Данные метаданных</param>
        /// <returns></returns>
        public Metadata MapToMetadata(string idCryptoModel, MetadataDto metadata);

        /// <summary>
        /// Выводит информацию для отображения с метаданными
        /// </summary>
        /// <param name="cryptoModels">Модели crypto</param>
        /// <returns></returns>
        public CryptoInfoDto[] GetCryptoWithLogo(Crypto[] cryptoModels);
    }
}