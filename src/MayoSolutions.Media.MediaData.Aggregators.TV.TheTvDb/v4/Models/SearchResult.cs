using System.Collections.Generic;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models
{
    public class SearchResult
    {
        [JsonProperty("aliases")]
        public List<string> Aliases { get; set; }

        [JsonProperty("companies")]
        public List<string> Companies { get; set; }

        [JsonProperty("companyType")]
        public string CompanyType { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("extendedTitle")]
        public string ExtendedTitle { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_translated")]
        public string NameTranslated { get; set; }

        [JsonProperty("network")]
        public string Network { get; set; }

        [JsonProperty("officialList")]
        public string OfficialList { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("overview_translated")]
        public List<string> OverviewTranslated { get; set; }
        
        [JsonProperty("posters")]
        public List<string> Posters { get; set; }

        [JsonProperty("primary_language")]
        public string PrimaryLanguage { get; set; }

        [JsonProperty("primary_type")]
        public string PrimaryType { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("translationsWithLang")]
        public List<string> TranslationsWithLang { get; set; }

        [JsonProperty("tvdb_id")]
        public string TheTvDbId { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

	}
}
