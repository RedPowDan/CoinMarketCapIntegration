namespace Domain.Dtos.Crypto
{
    public class CryptoListingLatestResponseDto
    {
        public StatusDto Status { get; set; }
        public DataOfListingDto[] Data { get; set; }
    }
}
