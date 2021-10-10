using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Domain.Dtos.Crypto;
using Newtonsoft.Json;

namespace Domain.Services
{
    using Interfaces;

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

        public void UpdateCryptoForAllCurrencies(CancellationToken stoppingToken)
        {
            _logger.LogInformation("UpdateCryptoForAllCurrencies started");

            var cryptoModels = GetCryptoModelsFromApi();
            UpdateOrCreateCrypto(cryptoModels);

            var cryptoIdsInApi = GetIdsInApi(cryptoModels);
            UpdateOrCreateCryptoCurrencyMetadata(cryptoIdsInApi);
        }

        public void UpdateOrCreateCrypto(Crypto[] newCryptoModels)
        {
            foreach (var cryptoModel in newCryptoModels)
            {
                UpdateOrCreateCrypto(cryptoModel);
            }

            _context.SaveChanges();
        }

        private void UpdateOrCreateCrypto(Crypto newCrypto)
        {
            var oldCrypto = _context.Cryptos.FirstOrDefault(x => x.IdInApi == newCrypto.IdInApi);

            if (oldCrypto == null)
            {
                _context.Cryptos.Add(newCrypto);
            }

            oldCrypto = newCrypto;
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
                modelsCrypto.Add(MapToCrypto(listingLatestData));
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
                DataUpdateTime = listingLatestData.Quote[CURRENCY_NAME].LastUpdated,
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