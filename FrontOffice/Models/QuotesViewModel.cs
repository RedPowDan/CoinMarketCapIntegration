using System;

namespace FrontOffice.Models
{
    using Domain.Dtos.Crypto;

    public class QuotesViewModel : PaginateView
    {
        private readonly int pageNumber;
        private readonly int totalPages;

        public CryptoInfoDto[] InfoCryptoModels { get; private set; }

        public QuotesViewModel(
            CryptoInfoDto[] infoCryptoModels,
            int count,
            int pageNumber, 
            int pageSize)
        {
            InfoCryptoModels = infoCryptoModels;
            this.pageNumber = pageNumber;
            totalPages = (int) Math.Ceiling(count / (double)pageSize);
        }

        public override int GetPageNumber()
        {
            return pageNumber;
        }

        public override int GetTotalPages()
        {
            return totalPages;
        }
    }
}