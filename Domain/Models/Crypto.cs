namespace Domain.Models
{
    using System;
    public class Crypto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal PercentChangePerHour { get; set; }
        public decimal PercentChangePerDay { get; set; }
        public decimal CapitalizationMarketCap { get; set; }
        public DateTime? DataUpdateTime { get; set; }
        public int IdInApi { get; set; }

        public void MapToModel(Crypto crypto)
        {
            crypto.Name = Name;
            crypto.Symbol = Symbol;
            crypto.Price = Price;
            crypto.PercentChangePerHour = PercentChangePerHour;
            crypto.PercentChangePerDay = PercentChangePerDay;
            crypto.DataUpdateTime = DataUpdateTime;
        }
    }
}