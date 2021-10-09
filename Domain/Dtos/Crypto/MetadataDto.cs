namespace Domain.Dtos.Crypto
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MetadataDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date_added")]
        public DateTime DateAdded { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("platform")]
        public PlatformDto Platform { get; set; }

        [JsonProperty("urls")]
        public Dictionary<string, List<string>> Urls { get; set; }
    }
}
