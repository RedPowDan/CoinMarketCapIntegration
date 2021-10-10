using Domain.Services.Interfaces;
using FrontOffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FrontOffice.Controllers
{
    public class CryptoController : Controller
    {
        private readonly int _countItemInPage;
        private readonly ICryptoService _cryptoService;
        private readonly IMetadataService _metadataService;
        public CryptoController(
            ICryptoService cryptoService,
            IMetadataService metadataService)
        {
            _countItemInPage = 20;
            _cryptoService = cryptoService;
            _metadataService = metadataService;
        }

        [Authorize]
        public ActionResult Quotes(
            string searchString,
            string sortOrder,
            int page = 1)
        {
            ViewBag.Name = "name";
            ViewBag.Symbol = "Symbol";
            ViewBag.Price = "Price";
            ViewBag.PercentChangePerHour = "PercentChangePerHour";
            ViewBag.PercentChangePerDay = "PercentChangePerDay";
            ViewBag.CapitalizationMarketCap = "CapitalizationMarketCap";

            var cryptoModels = _cryptoService.GetCryptoModelsWithFilters(
                searchString,
                sortOrder);

            var paginateCryptoModels = _cryptoService.GetCryptoInfosWithPaginate(
                cryptoModels,
                _countItemInPage,
                page);

            var infoCryptoModels = _metadataService.GetCryptoWithLogo(paginateCryptoModels);

            var model = new QuotesViewModel(
                infoCryptoModels,
                cryptoModels.Length,
                page,
                _countItemInPage);

            ViewData["sort"] = sortOrder;

            return View(model);
        }
    }
}