using System.Collections.Generic;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    public class SeriesBaseRecord
    {
        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("aliases")]
        public List<Alias> Aliases { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("defaultSeasonType")]
        public long? DefaultSeasonType { get; set; }

        [JsonProperty("firstAired")]
        public string FirstAired { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("isOrderRandomized")]
        public bool IsOrderRandomized { get; set; }

        [JsonProperty("lastAired")]
        public string LastAired { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameTranslations")]
        public List<string> NameTranslations { get; set; }

        [JsonProperty("nextAired")]
        public string NextAired { get; set; }

        [JsonProperty("originalCountry")]
        public string OriginalCountry { get; set; }

        [JsonProperty("originalLanguage")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("overviewTranslations")]
        public List<string> OverviewTranslations { get; set; }

        [JsonProperty("score")]
        public double? Score { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

	}
}