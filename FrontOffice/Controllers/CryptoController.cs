using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FrontOffice.Controllers
{
    public class CryptoController : Controller
    {
        private readonly int _countItemInPage;
        private readonly ICryptoService _cryptoService;
        public CryptoController(ICryptoService cryptoService)
        {
            _countItemInPage = 20;
            _cryptoService = cryptoService;
        }

        [Authorize]
        public ActionResult Quotes(
            string searchString,
            string sortOrder,
            int pageNumber = 1)
        {
            var infoCryptoModels = _cryptoService.GetCryptoInfosWithFilters(
                searchString,
                sortOrder,
                _countItemInPage,
                pageNumber);

            ViewData["sort"] = sortOrder;

            return View(infoCryptoModels);
        }
    }
}