namespace Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Models;
    using Microsoft.Extensions.Logging;
    using System.Net.Http;
    using System.Web;
    using Dtos.Crypto;
    using Newtonsoft.Json;
    using Interfaces;
    using PagedList;

    /// <summary>
    /// Сервис с криптовалютой. Загрузка, создание, обновление.
    /// </summary>
    public class CryptoService : ICryptoService
    {
        private readonly ILogger<CryptoService> _logger;
        private readonly DomainContext _context;
        private readonly IMetadataService _metadataService;
        private static string API_KEY = "7e3abbd7-d8b7-470b-82a6-1adcfd570c6f";
        private const string CURRENCY_NAME = "USD";

        public CryptoService(
            ILogger<CryptoService> logger,
            DomainContext context,
            IMetadataService metadataService)
        {
            _logger = logger;
            _context = context;
            _metadataService = metadataService;
        }

        /// <summary>
        /// Возвращает количество моделей в таблице crypto
        /// </summary>
        /// <returns></returns>
        public int GetCountCryptoModelsInDb()
        {
            return _context.Cryptos.Count();
        }

        /// <summary>
        /// Получает айдишники моделей в API 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public int[] GetIdsInApi(Crypto[] models)
        {
            if (models == null && models.Length == 0)
            {
                return null;
            }

            return models
                .GroupBy(x => x.IdInApi)
                .Select(y => y.Key)
                .ToArray();
        }

        /// <summary>
        /// Обновляет, либо создает модели
        /// </summary>
        /// <param name="models"></param>
        public void UpdateOrCreateCrypto(Crypto[] newCryptoModels)
        {
            foreach (var cryptoModel in newCryptoModels)
            {
                UpdateOrCreateCrypto(cryptoModel);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Используется для загрузки, создания, обновления информации про криптовалюту
        /// </summary>
        public void UpdateCryptoForAllCurrencies()
        {
            _logger.LogInformation("UpdateCryptoForAllCurrencies started");

            var cryptoModels = GetCryptoModelsFromApi();
            UpdateOrCreateCrypto(cryptoModels);

            var cryptoIdsInApi = GetIdsInApi(cryptoModels);
            UpdateOrCreateCryptoCurrencyMetadata(cryptoIdsInApi);
        }

        public Crypto[] GetCryptoModelsWithFiltersAndPaginate(
            string searchString,
            string sortOrder,
            int countOutput,
            int pageNumber)
        {
            // TODO: в дальнейшей реализации сделать сортировку с помощью массива аргументов

            var cryptos = _context.Cryptos.Where(x => true);

            if (!String.IsNullOrEmpty(searchString))
            {
                cryptos = cryptos.Where(c => c.Name.Contains(searchString)
                                             || c.Symbol.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name":
                    cryptos = cryptos.OrderByDescending(c => c.Name);
                    break;
                case "Symbol":
                    cryptos = cryptos.OrderByDescending(c => c.Symbol);
                    break;
                case "Price":
                    cryptos = cryptos.OrderByDescending(c => c.Price);
                    break;
                case "PercentChangePerHour":
                    cryptos = cryptos.OrderByDescending(c => c.PercentChangePerHour);
                    break;
                case "PercentChangePerDay":
                    cryptos = cryptos.OrderByDescending(c => c.PercentChangePerDay);
                    break;
                case "CapitalizationMarketCap":
                    cryptos = cryptos.OrderByDescending(c => c.CapitalizationMarketCap);
                    break;
            }

            return cryptos.ToPagedList(pageNumber, countOutput).ToArray();
        }

        private void UpdateOrCreateCrypto(Crypto newCrypto)
        {
            var oldCrypto = _context.Cryptos.FirstOrDefault(x => x.IdInApi == newCrypto.IdInApi);

            if (oldCrypto == null)
            {
                _context.Cryptos.Add(newCrypto);
                return;
            }

            newCrypto.MapToModel(oldCrypto);
        }

        private Crypto[] GetCryptoModelsFromApi()
        {
            var listingLatestResponse = GetCryptoListingLatestResponse();

            var errorCode = listingLatestResponse.Status.ErrorCode;
            if (errorCode != 0)
            {
                _logger.LogInformation($"Crypto is not download {errorCode}," +
                                       $" error message = {listingLatestResponse.Status.ErrorMessage}");
                return null;
            }

            var modelsCrypto = new List<Crypto>();
            foreach (var listingLatestData in listingLatestResponse.Data)
            {
                var mapCryptoModel = MapToCrypto(listingLatestData);
                mapCryptoModel.DataUpdateTime = DateTime.Now; // TODO: Лучше сделать сервис котоырй будет выводить дату (по городу и тд.)
                modelsCrypto.Add(mapCryptoModel);
            }

            return modelsCrypto.ToArray();
        }

        private CryptoListingLatestResponseDto GetCryptoListingLatestResponse()
        {
            CryptoListingLatestResponseDto listingLatestResponse = null;
            try
            {
                using var httpClient = new HttpClient();
                var uriBuilder = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

                var queryString = HttpUtility.ParseQueryString(string.Empty);

                uriBuilder.Query = queryString.ToString();

                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
                client.Headers.Add("Accepts", "application/json");

                var responseMessage = client.DownloadString(uriBuilder.ToString());

                listingLatestResponse = DeserializeListingLatestFromString(responseMessage);
            }
            catch (Exception es)
            {
                throw new Exception("Error in download data");
            }

            return listingLatestResponse;
        }

        private void UpdateOrCreateCryptoCurrencyMetadata(int[] cryptoIdsInApi)
        {
            var metadataResponse = GetMetadataResponse(cryptoIdsInApi);

            var errorCode = metadataResponse.Status.ErrorCode;
            if (errorCode != 0)
            {
                _logger.LogInformation($"Metadata is not download {errorCode}," +
                                             $" error message = {metadataResponse.Status.ErrorMessage}");
                return;
            }

            var modelsMetadata = new List<Metadata>();
            foreach (var keyMetaData in metadataResponse.Data.Keys)
            {
                var metadataModel = _metadataService.MapToMetadata(
                                                        keyMetaData,
                                                        metadataResponse.Data[keyMetaData]);
                if(metadataModel != null)
                    modelsMetadata.Add(metadataModel);
            }

            _metadataService.UpdateOrCreateMetadataForModels(modelsMetadata.ToArray());
        }

        private MetadataResponseDto GetMetadataResponse(int[] cryptoIdsInApi)
        {
            var uri = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/info");
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            uri.Query = GetRequestFromMassInt(cryptoIdsInApi) ?? string.Empty;

            var client = new WebClient();
            client.Headers["X-CMC_PRO_API_KEY"] = API_KEY;
            client.Headers["Accepts"] = "application/json";

            var responseMetadata = client.DownloadString(uri.ToString());
            return DeserializeMetadataFromString(responseMetadata);
        }

        private Crypto MapToCrypto(DataOfListingDto listingLatestData)
        {
            return new Crypto()
            {
                Name = listingLatestData.Name,
                Symbol = listingLatestData.Symbol,
                Price = listingLatestData.Quote[CURRENCY_NAME].Price,
                PercentChangePerHour = listingLatestData.Quote[CURRENCY_NAME].PercentChangeHour,
                PercentChangePerDay = listingLatestData.Quote[CURRENCY_NAME].PercentChangeDay,
                CapitalizationMarketCap = listingLatestData.Quote[CURRENCY_NAME].MarketCap,
                IdInApi = listingLatestData.Id
            };
        }

        private MetadataResponseDto DeserializeMetadataFromString(string dataJson)
        {
            var outputData = JsonConvert.DeserializeObject<MetadataResponseDto>(dataJson, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return outputData;
        }

        private CryptoListingLatestResponseDto DeserializeListingLatestFromString(string dataJson)
        {
            var outputData = JsonConvert.DeserializeObject<CryptoListingLatestResponseDto>(dataJson, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return outputData;
        }

        private string GetRequestFromMassInt(int[] mass)
        {
            if (mass == null || mass.Length == 0)
            {
                return null;
            }

            string outputString = mass[0].ToString();
            for (int i = 1; i < mass.Length; i++)
            {
                outputString += $",{mass[i]}";
            }

            var requestString = "id=" + outputString;

            return requestString;
        }
    }
}