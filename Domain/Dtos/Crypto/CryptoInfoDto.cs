namespace Domain.Dtos.Crypto
{
    using System;
    public class CryptoInfoDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Logo { get; set; }
        public decimal CurrentPriceUsd { get; set; }
        public decimal PercentChangePerHour { get; set; }
        public decimal PercentChangePerDay { get; set; }
        public decimal CapitalizationMarketCap { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}