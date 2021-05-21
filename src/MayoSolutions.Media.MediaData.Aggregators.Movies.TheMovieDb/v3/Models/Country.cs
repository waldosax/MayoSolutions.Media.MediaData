using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3.Models
{
    internal class Country
    {
        [JsonProperty("iso_3166_1")]
        public string Iso31661 { get; set; }
        public string Name { get; set; }
    }
}