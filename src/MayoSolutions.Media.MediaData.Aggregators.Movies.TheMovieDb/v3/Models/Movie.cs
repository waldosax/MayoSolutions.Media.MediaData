using System;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3.Models
{
    internal class Movie
    {
        public bool Adult { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("belongs_to_collection")]
        public object BelongsToCollection { get; set; }

        public int Budget { get; set; }

        [JsonProperty("genre_ids")]
        public int[] GenreIds { get; set; }
        public Genre[] Genres { get; set; }
        public string Homepage { get; set; }
        public int Id { get; set; }

        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }
        
        public string Overview { get; set; }
        public float Popularity{ get; set; }
        
        [JsonProperty("poster_path")]
        public string PosterPath{ get; set; }
        
        [JsonProperty("production_companies")]
        public ProductionCompany[] ProductionCompanies { get; set; }
        
        [JsonProperty("production_countries")]
        public Country[] ProductionCountries { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        public int Revenue { get; set; }
        public int? Runtime { get; set; }

        [JsonProperty("spoken_languages")]
        public Language[] SpokenLanguages{ get; set; }
        
        public string Status { get; set; }
        public string Tagline { get; set; }
        public string Title { get; set; }
        public bool Video{ get; set; }

        [JsonProperty("vote_average")]
        public float VoteAverage{ get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount{ get; set; }

    }
}
