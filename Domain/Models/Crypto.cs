using System;

namespace Domain.Models
{
    public class Crypto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Logo { get; set; }
        public float Price { get; set; }
        public float PriceChangePerHour { get; set; }
        public float PriceChangePerDay { get; set; }
        public float MarketCap { get; set; }
        public DateTime DataUpdateTime { get; set; }
    }
}