using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3.Models
{
    internal class Language
    {
        [JsonProperty("iso_639_1")]
        public string Iso6391 { get; set; }
        public string Name { get; set; }
    }
}