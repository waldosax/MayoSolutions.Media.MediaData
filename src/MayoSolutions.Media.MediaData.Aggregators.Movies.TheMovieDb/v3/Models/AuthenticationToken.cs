using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3.Models
{
    internal class AuthenticationToken
    {
        public bool Success { get; set; }

        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }

        [JsonProperty("request_token")]
        public string RequesttToken { get; set; }

    }
}
