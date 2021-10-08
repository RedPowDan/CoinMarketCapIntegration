using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Web;

namespace Domain.Services
{
    using Interfaces;

    public class CryptoService : ICryptoService
    {
        private readonly ILogger<CryptoService> _logger;
        private readonly DomainContext _context;
        private static string API_KEY = "7e3abbd7-d8b7-470b-82a6-1adcfd570c6f";

        public CryptoService(
            ILogger<CryptoService> logger,
            DomainContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task UpdateCryptoForAllCurrencies(CancellationToken stoppingToken)
        {
            _logger.LogInformation("UpdateCryptoForAllCurrencies started");

            using var httpClient = new HttpClient();
            var uriBuilder = new UriBuilder("https://pro-api.coinmarketcap.com/v1/global-metrics/quotes/historical");

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["hourly"] = "1";
            queryString["limit"] = "5000";
            queryString["convert"] = "USD";

            uriBuilder.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
            client.Headers.Add("Accepts", "application/json");
            var responseMessage = client.DownloadString(uriBuilder.ToString());

        }

        private Task< GetStringResponse
    }
}