using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MayoSolutions.Common.Extensions;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3.Models;
using MayoSolutions.Media.MediaData.Movies;
using Newtonsoft.Json;
using Movie = MayoSolutions.Media.MediaData.Movies.Movie;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3
{
    /// <summary>
    /// Aggregates data from The MovieDB API and converts them into objects used by the library.
    /// </summary>
    [Aggregator("TheMovieDB.com")]
    public class TheMovieDbEpisodeDataProvider : IMovieAggregator
    {
        private const int MaxSearchResults = 10;
        private const int MaxImageResults = 5;
        private readonly ITheMovieDbApiDownloader _apiDownloader;

        public TheMovieDbEpisodeDataProvider(
            ITheMovieDbApiDownloader apiDownloader
        )
        {
            _apiDownloader = apiDownloader;
        }


        public virtual async Task<Movie[]> SearchAsync(MovieSearchParams movieSearchParams,
            IWebProxy proxy)
        {
            // TODO: IMDB Id
            string json = await _apiDownloader.GetMovieSearchResultsAsync(movieSearchParams, proxy);
            var searchResults = Deserialize<SearchResults<Models.Movie>>(json);

            Models.Movie[] data = searchResults.Results?.Take(MaxSearchResults).ToArray();
            // TODO: Paging
            
            List<Movie> movieResults = data?
                .Select(x => Adapt(x))
                .ToList();
            return movieResults?.ToArray() ?? new Movie[0];
        }



        public virtual async Task<Movie> GetMovieDetailsAsync(IMovieIdentifier movieIdentifier,
            IWebProxy proxy)
        {
            string json =
                await _apiDownloader.GetMovieDetailsAsync(movieIdentifier, proxy);
            var movieResponse = Deserialize<Models.Movie>(json);

            Movie movie = Adapt(movieResponse);
            return movie;
        }
        

        private T Deserialize<T>(string json)
        {
            // TODO: Errors and serialization exceptions
            return JsonConvert.DeserializeObject<T>(json);
        }

        private static class MovieGenres
        {
            public static readonly MovieGenre Unknown = RegisterGenre(0, "Unknown");
            public static readonly MovieGenre Action = RegisterGenre(28, "Action");
            public static readonly MovieGenre Adventure = RegisterGenre(12, "Adventure");
            public static readonly MovieGenre Animation = RegisterGenre(16, "Animation");
            public static readonly MovieGenre Comedy = RegisterGenre(35, "Comedy");
            public static readonly MovieGenre Crime = RegisterGenre(80, "Crime");
            public static readonly MovieGenre Documentary = RegisterGenre(99, "Documentary");
            public static readonly MovieGenre Drama = RegisterGenre(18, "Drama");
            public static readonly MovieGenre Family = RegisterGenre(10751, "Family");
            public static readonly MovieGenre Fantasy = RegisterGenre(14, "Fantasy");
            public static readonly MovieGenre History = RegisterGenre(36, "History");
            public static readonly MovieGenre Horror = RegisterGenre(27, "Horror");
            public static readonly MovieGenre Music = RegisterGenre(10402, "Music");
            public static readonly MovieGenre Mystery = RegisterGenre(9648, "Mystery");
            public static readonly MovieGenre Romance = RegisterGenre(10749, "Romance");
            public static readonly MovieGenre ScienceFiction = RegisterGenre(878, "Science Fiction");
            public static readonly MovieGenre TVMovie = RegisterGenre(10770, "TV Movie");
            public static readonly MovieGenre Thriller = RegisterGenre(53, "Thriller");
            public static readonly MovieGenre War = RegisterGenre(10752, "War");
            public static readonly MovieGenre Western = RegisterGenre(37, "Western");

            private static readonly Dictionary<int, MovieGenre> _movieGenres = new Dictionary<int, MovieGenre>();
            private static MovieGenre RegisterGenre(int id, string name)
            {
                var genre = new MovieGenre(id, name);
                _movieGenres.Add(id, genre);
                return genre;
            }

            public static MovieGenre Get(int id)
            {
                if (_movieGenres.ContainsKey(id)) return _movieGenres[id];
                return Unknown;
            }
        }
        private struct MovieGenre
        {
            public int Id { get; }
            public string Name { get; }

            internal MovieGenre(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }

            public static implicit operator string(MovieGenre genre)
            {
                return genre.ToString();
            }

            public static implicit operator int(MovieGenre genre)
            {
                return genre.Id;
            }
        }

        #region Adapt

        private Movie Adapt(Models.Movie input)
        {
            Adapt(input, out Movie movie);

            // TODO: Poster URLs

            return movie;
        }

        private void Adapt(Models.Movie input, out Movie output)
        {
            output = new Movie
            {
                Id = input.Id.ToString(),
                Name = input.Title,
                ImdbId = input.ImdbId,
                Description = input.Overview,
                ReleaseDate = input.ReleaseDate.ToNullableDateTime("yyyy-MM-dd"),
                Year = input.ReleaseDate.ToNullableDateTime("yyyy-MM-dd")?.Year,
            };
            
            Adapt(input.Genres, input.GenreIds, out var genres);
            output.Genres = genres;

            // TODO: Production Companies

        }

        private void Adapt(Models.Genre[] genres, int[] genreIds, out string[] output)
        {
            output = new string[0];
            List<string> list = new List<string>();

            if (genres != null)
            {
                list.AddRange(genres
                    .Select(g => g.Name)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray());
            }

            if (genreIds != null)
            {
                list.AddRange(genreIds
                    .Select(id => MovieGenres.Get(id).Name)
                    .Except(new []{MovieGenres.Unknown.Name})
                    .Except(list)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray());
            }

            output = list
                .ToArray();
        }

        #endregion
    }
}
