using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MayoSolutions.Framework;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.TV;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2.Models;
using MayoSolutions.Media.MediaData.TV;
using Newtonsoft.Json;
using NUnit.Framework;
using Tests.Shared.Configuration;
using Tests.Shared.TheTvDb;

namespace TheTvDb.BenchTests.v2
{
    public class ApiTests
    {
        protected ITheTvDbApiDownloader CreateDownloader()
        {
            var configurationValues = new TheTvDbConfigurationValues(new TestConfigurationValues());
            return new TheTvDbApiDownloader(configurationValues, new HttpDownloader());
        }

        protected async Task<string> GetAuthTokenAsync()
        {
            var configurationValues = new TheTvDbConfigurationValues(new TestConfigurationValues());
            var localPaths = new TestLocalPaths();
            var apiDownloader = CreateDownloader();
            var authenticator =
                new TheTvDbAuthenticator(new RealTimeDateProvider(), configurationValues, localPaths, apiDownloader);
            return await authenticator.GetAuthTokenAsync(null);
        }

        protected ISeriesIdentifier GetSeriesIdentifier(KnownTvShowIds tvdbShowId)
        {
            return new SeriesIdentifier { Id = tvdbShowId.ToString("D") };
        }

        [Test]
        public async Task ShouldProduceSearchResults()
        {
            // Arrange
            var searchParams = new SeriesSearchParams { Name = "The Bad Batch" };
            var authToken = await GetAuthTokenAsync();
            var sut = CreateDownloader();

            // Act
            var actual = await sut.GetSeriesSearchResultsAsync(searchParams, authToken, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            TheTvDbSeriesSearchResult response =
                JsonConvert.DeserializeObject<TheTvDbSeriesSearchResult>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Length, Is.GreaterThan(0));
            Assert.That(response.Data[0], Is.Not.Null);
            Assert.That(response.Data[0].SeriesName, Is.Not.Null);
            Assert.That(response.Data[0].SeriesName, Is.Not.Empty);
            Assert.That(response.Data[0].SeriesName, Is.EqualTo("The Bad Batch"));
            Assert.That(response.Data[0].Id, Is.EqualTo((int)KnownTvShowIds.StarWarsTheBadBatch));
        }

        [Test]
        public async Task ShouldProduceSeriesInfo()
        {
            // Arrange
            var seriesId = GetSeriesIdentifier(KnownTvShowIds.StarWarsTheBadBatch);
            var authToken = await GetAuthTokenAsync();
            var sut = CreateDownloader();

            // Act
            var actual = await sut.GetSeriesAsync(seriesId, authToken, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            TheTvDbSeriesResult response =
                JsonConvert.DeserializeObject<TheTvDbSeriesResult>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Errors, Is.Null | Is.Empty);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo((int)KnownTvShowIds.StarWarsTheBadBatch));
        }

        [Test]
        public async Task ShouldProduceEpisodes()
        {
            // Arrange
            var seriesId = GetSeriesIdentifier(KnownTvShowIds.StarWarsTheBadBatch);
            var authToken = await GetAuthTokenAsync();
            var sut = CreateDownloader();

            // Act
            var actual = await sut.GetEpisodesAsync(seriesId, authToken, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            Console.WriteLine(actual);
            TheTvDbEpisodeResult response =
                JsonConvert.DeserializeObject<TheTvDbEpisodeResult>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Errors, Is.Null | Is.Empty);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Length, Is.GreaterThan(0));
            Assert.That(response.Data[0], Is.Not.Null);
            Assert.That(response.Data[0].SeriesId, Is.EqualTo(KnownTvShowIds.StarWarsTheBadBatch.ToString("D")));
            Assert.That(response.Data[0].EpisodeName, Is.EqualTo("Aftermath"));
        }
    }
}
