using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3.Models
{
    internal class FindResults
    {
        [JsonProperty("movie_results")]
        public Movie[] MovieResults { get; set; }

        //TODO; Other results types (TV, People, etc.)
    }
}
