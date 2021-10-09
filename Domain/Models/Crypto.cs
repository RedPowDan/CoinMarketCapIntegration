using System;

namespace Domain.Models
{
    public class Crypto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal PercentChangePerHour { get; set; }
        public decimal PercentChangePerDay { get; set; }
        public decimal CapitalizationMarketCap { get; set; }
        public DateTime DataUpdateTime { get; set; }
        public int IdInApi { get; set; }
    }
}