using System;
using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.Movies;
using MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3;
using MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3.Models;
using MayoSolutions.Media.MediaData.Movies;
using Newtonsoft.Json;
using NUnit.Framework;
using Tests.Shared.Configuration;
using Tests.Shared.TheMovieDb;
using Models = MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3.Models;

namespace TheMovieDb.BenchTests.v3
{
    public class ApiTests
    {
        protected ITheMovieDbApiDownloader CreateDownloader()
        {
            var configurationValues = new TheMovieDbConfigurationValues(new TestConfigurationValues());
            return new TheMovieDbApiDownloader(configurationValues, new HttpDownloader());
        }


        protected IMovieIdentifier GetMovieIdentifier(KnownMovieIds movieId)
        {
            return new MovieIdentifier { Id = movieId.ToString("D") };
        }

        [Test]
        public async Task ShouldProduceSearchResults()
        {
            // Arrange
            var searchParams = new MovieSearchParams { Name = "The Godfather" };
            var sut = CreateDownloader();

            // Act
            var actual = await sut.GetMovieSearchResultsAsync(searchParams, 1, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            SearchResults<Models.Movie> response =
                JsonConvert.DeserializeObject<SearchResults<Models.Movie>>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results, Is.Not.Null | Is.Not.Empty);
            Assert.That(response.Results[0], Is.Not.Null);
            Assert.That(response.Results[0].Title, Is.Not.Null);
            Assert.That(response.Results[0].Title, Is.Not.Empty);
            Assert.That(response.Results[0].Title, Is.EqualTo("The Godfather"));
            Assert.That(response.Results[0].Id, Is.EqualTo((int)KnownMovieIds.TheGodfather));
        }

        [Test]
        public async Task ShouldProduceMovieDetails()
        {
            // Arrange
            var movieId = GetMovieIdentifier(KnownMovieIds.TheGodfather);
            var sut = CreateDownloader();

            // Act
            var actual = await sut.GetMovieDetailsAsync(movieId, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            Models.Movie response =
                JsonConvert.DeserializeObject<Models.Movie>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id, Is.EqualTo((int)KnownMovieIds.TheGodfather));
            Assert.That(response.Status, Is.EqualTo("Released"));
        }

        [Test]
        public async Task ShouldFindMovieByImdbId()
        {
            // Arrange
            var imdbId = "tt0068646";
            var searchParams = new MovieSearchParams { ImdbId = imdbId };
            var sut = CreateDownloader();

            // Act
            var actual = await sut.FindByExternalId(searchParams, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            Console.WriteLine(actual);
            FindResults response =
                JsonConvert.DeserializeObject<FindResults>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.MovieResults, Is.Not.Null | Is.Not.Empty);
            Assert.That(response.MovieResults[0], Is.Not.Null);
            Assert.That(response.MovieResults[0].Id, Is.EqualTo((int)KnownMovieIds.TheGodfather));
        }
    }
}
