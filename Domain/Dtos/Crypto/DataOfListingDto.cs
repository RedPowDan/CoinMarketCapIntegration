namespace Domain.Dtos.Crypto
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class DataOfListingDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("cmc_rank")]
        public int CmcRank { get; set; }

        [JsonProperty("num_market_pairs")]
        public int NumMarketPairs { get; set; }

        [JsonProperty("circulating_supply")]
        public decimal CirculatingSupply { get; set; }

        [JsonProperty("total_supply")]
        public decimal TotalSupply { get; set; }

        [JsonProperty("market_cap_by_total_supply")]
        public decimal MarketCapByTotalSupply { get; set; }

        [JsonProperty("max_supply")]
        public decimal MaxSupply { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("date_added")]
        public DateTime DateAdded { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("platform")]
        public PlatformDto Platform { get; set; }

        [JsonProperty("quote")]
        public Dictionary<string, QuoteDto> Quote { get; set; }
    }
}
