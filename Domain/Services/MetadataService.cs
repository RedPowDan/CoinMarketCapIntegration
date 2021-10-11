namespace Domain.Services
{
    using Interfaces;
    using System.Linq;
    using Models;
    using System.Collections.Generic;
    using Dtos.Crypto;
    using Microsoft.Extensions.Logging;

    public class MetadataService : IMetadataService
    {
        private readonly DomainContext _context;
        private readonly ILogger<MetadataService> _logger;
        public MetadataService(
            DomainContext context,
            ILogger<MetadataService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Выводит массив моделей по айдишникам
        /// </summary>
        /// <param name="Ids">Айдишники моделей</param>
        /// <returns></returns>
        public Metadata[] GetMetadataForIds(int[] Ids)
        {
            if (Ids == null)
            {
                return null;
            }

            return _context
                    .Metadata
                    .Where(x => Ids.Contains(x.Id))
                    .ToArray();
        }

        /// <summary>
        /// Обновляет или создает модели
        /// </summary>
        /// <param name="newMetadataModels">Новые модели метаданных</param>
        public void UpdateOrCreateMetadataForModels(Metadata[] newMetadataModels)
        {
            foreach (var newMetadata in newMetadataModels)
            {
                UpdateOrCreateMetadata(newMetadata);
            }

            _context.SaveChanges();
        }

        private void UpdateOrCreateMetadata(Metadata newMetadata)
        {
            var oldMetadata = _context.Metadata
                .FirstOrDefault(x => x.Crypto.ID == newMetadata.Crypto.ID);

            if (oldMetadata == null)
            {
                _context.Metadata.Add(newMetadata);
                return;
            }

            newMetadata.MapToModel(oldMetadata);
        }

        /// <summary>
        /// Получает айдишники моделей
        /// </summary>
        /// <param name="models">Модели</param>
        /// <returns></returns>
        public int[] GetIds(Metadata[] models)
        {
            if (models == null)
            {
                return null;
            }

            return models
                .GroupBy(x => x.Id)
                .Select(y => y.Key)
                .ToArray();
        }

        /// <summary>
        /// Получает айдишники модели crypto 
        /// </summary>
        /// <param name="models">Модели</param>
        /// <returns></returns>
        public int[] GetCryptoIds(Metadata[] models)
        {
            if (models == null)
            {
                return null;
            }

            return models
                .GroupBy(x => x.Crypto.ID)
                .Select(y => y.Key)
                .ToArray();
        }

        /// <summary>
        /// Выводит новую мдель метаданных
        /// </summary>
        /// <param name="idCryptoModel">Айди модели crypto</param>
        /// <param name="metadata">Данные метаданных</param>
        /// <returns></returns>
        public Metadata MapToMetadata(string idCryptoModel, MetadataDto metadata)
        {
            var id = int.Parse(idCryptoModel);
            var cryptoModel = _context.Cryptos.FirstOrDefault(x => x.IdInApi == id);

            if (cryptoModel == null)
            {
                _logger.LogInformation($"CryptoModel is not found with id = {id}");
                return null;
            }

            return new Metadata()
            {
                Crypto = cryptoModel,
                LogoUri = metadata.Logo
            };
        }

        /// <summary>
        /// Выводит информацию для отображения с метаданными
        /// </summary>
        /// <param name="cryptoModels">Модели crypto</param>
        /// <returns></returns>
        public CryptoInfoDto[] GetCryptoWithLogo(Crypto[] cryptoModels)
        {
            var idsCryptoInApi = cryptoModels
                .GroupBy(x => x.ID)
                .Select(y => y.Key)
                .ToArray();

            var metadataModelsForCryptoModels = _context.Metadata
                .Where(x => idsCryptoInApi.Contains(x.Crypto.ID));

            var cryptoInfos = new List<CryptoInfoDto>();

            foreach (var cryptoModel in cryptoModels)
            {
                var metadataForModel = metadataModelsForCryptoModels
                    .FirstOrDefault(x => x.Crypto.ID == cryptoModel.ID);
                if (metadataForModel != null)
                {
                    cryptoInfos.Add(new CryptoInfoDto
                    {
                        Name = cryptoModel.Name,
                        Symbol = cryptoModel.Symbol,
                        Logo = metadataForModel.LogoUri,
                        CurrentPriceUsd = cryptoModel.Price,
                        PercentChangePerDay = cryptoModel.PercentChangePerDay,
                        PercentChangePerHour = cryptoModel.PercentChangePerHour,
                        CapitalizationMarketCap = cryptoModel.CapitalizationMarketCap,
                        UpdateTime = cryptoModel.DataUpdateTime.GetValueOrDefault()
                    });
                }
            }

            return cryptoInfos.ToArray();
        }
    }
}