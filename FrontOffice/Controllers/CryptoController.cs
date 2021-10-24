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

            var paginateCryptoModels = _cryptoService.GetCryptoModelsWithFiltersAndPaginate(
                searchString,
                sortOrder,
                _countItemInPage,
                page);

            var infoCryptoModels = _metadataService.GetCryptoWithLogo(paginateCryptoModels);

            var model = new QuotesViewModel(
                infoCryptoModels,
                _cryptoService.GetCountCryptoModelsInDb(),
                page,
                _countItemInPage);

            ViewData["searchString"] = searchString;
            
            ViewData["sort"] = sortOrder;

            return View(model);
        }
    }
}