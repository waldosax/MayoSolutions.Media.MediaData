using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MayoSolutions.Framework;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.TV;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models;
using MayoSolutions.Media.MediaData.TV;
using Newtonsoft.Json;
using NUnit.Framework;
using Tests.Shared.Configuration;
using Tests.Shared.TheTvDb;

namespace TheTvDb.BenchTests.v4
{
    public class ApiTests
    {
        protected ITheTvDbApiDownloaderV4 CreateDownloader()
        {
            var configurationValues = new TheTvDbConfigurationValuesV4(new TestConfigurationValues());
            return new TheTvDbApiDownloaderV4(configurationValues, new HttpDownloader());
        }

        protected async Task<string> GetAuthTokenAsync()
        {
            var configurationValues = new TheTvDbConfigurationValuesV4(new TestConfigurationValues());
            var localPaths = new TestLocalPaths();
            var apiDownloader = CreateDownloader();
            var authenticator =
                new TheTvDbAuthenticatorV4(new RealTimeDateProvider(), configurationValues, localPaths, apiDownloader);
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

            TheTvDbApiResponse<List<SearchResult>> response =
                JsonConvert.DeserializeObject<TheTvDbApiResponse<List<SearchResult>>>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Status, Is.EqualTo("success"));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data, Has.Count.GreaterThan(0));
            Assert.That(response.Data[0], Is.Not.Null);
            Assert.That(response.Data[0].Name, Is.Not.Null);
            Assert.That(response.Data[0].Name, Is.Not.Empty);
            Assert.That(response.Data[0].Name, Is.EqualTo("The Bad Batch"));
            Assert.That(response.Data[0].Type, Is.Not.Null);
            Assert.That(response.Data[0].Type, Is.Not.Empty);
            Assert.That(response.Data[0].Type, Is.EqualTo("series"));
            Assert.That(response.Data[0].TheTvDbId, Is.Not.Null);
            Assert.That(response.Data[0].TheTvDbId, Is.Not.Empty);
            Assert.That(response.Data[0].TheTvDbId, Is.EqualTo("385376"));
            Assert.That(response.Data[0].Id, Is.Not.Null);
            Assert.That(response.Data[0].Id, Is.Not.Empty);
            Assert.That(response.Data[0].Id, Is.EqualTo("series-385376"));
        }

        [Test]
        public async Task ShouldProduceSeriesInfo()
        {
            // Arrange
            var seriesId = GetSeriesIdentifier(KnownTvShowIds.StarWarsTheBadBatch);
            var authToken = await GetAuthTokenAsync();
            var sut = CreateDownloader();

            // Act
            var actual = await sut.GetSeriesExtendedAsync(seriesId, authToken, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            TheTvDbApiResponse<SeriesBaseRecord> response =
                JsonConvert.DeserializeObject<TheTvDbApiResponse<SeriesBaseRecord>>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Status, Is.EqualTo("success"));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(long.Parse(KnownTvShowIds.StarWarsTheBadBatch.ToString("D"))));
        }

        [Test]
        public async Task ShouldProduceSeriesExtendedInfo()
        {
            // Arrange
            var seriesId = GetSeriesIdentifier(KnownTvShowIds.StarWarsTheBadBatch);
            var authToken = await GetAuthTokenAsync();
            var sut = CreateDownloader();

            // Act
            var actual = await sut.GetSeriesExtendedAsync(seriesId, authToken, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            Console.WriteLine(actual);
            TheTvDbApiResponse<SeriesExtendedRecord> response =
                JsonConvert.DeserializeObject<TheTvDbApiResponse<SeriesExtendedRecord>>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Status, Is.EqualTo("success"));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(long.Parse(KnownTvShowIds.StarWarsTheBadBatch.ToString("D"))));
        }

        [Test]
        public async Task ShouldProduceSeasonExtendedInfo()
        {
            // Arrange
            var seriesId = GetSeriesIdentifier(KnownTvShowIds.StarWarsTheBadBatch);
            var authToken = await GetAuthTokenAsync();
            var sut = CreateDownloader();
            var seriesJson = await sut.GetSeriesExtendedAsync(seriesId, authToken, null);
            TheTvDbApiResponse<SeriesExtendedRecord> series =
                JsonConvert.DeserializeObject<TheTvDbApiResponse<SeriesExtendedRecord>>(seriesJson);
            var season = series.Data.Seasons.First(x => x.Number == 1 && x.Name == "Aired Order");

            // Act
            var actual = await sut.GetSeasonExtendedAsync(season.Id, authToken, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            TheTvDbApiResponse<SeasonExtendedRecord> response =
                JsonConvert.DeserializeObject<TheTvDbApiResponse<SeasonExtendedRecord>>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Status, Is.EqualTo("success"));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.Not.Null);
            Assert.That(response.Data.SeriesId, Is.EqualTo(long.Parse(KnownTvShowIds.StarWarsTheBadBatch.ToString("D"))));
            Assert.That(response.Data.Id, Is.EqualTo(season.Id));
        }

        [Test]
        public async Task ShouldProduceEpisodeTranslation()
        {
            // Arrange
            var seriesId = GetSeriesIdentifier(KnownTvShowIds.StarWarsTheBadBatch);
            var authToken = await GetAuthTokenAsync();
            var sut = CreateDownloader();
            var seriesJson = await sut.GetSeriesExtendedAsync(seriesId, authToken, null);
            TheTvDbApiResponse<SeriesExtendedRecord> series =
                JsonConvert.DeserializeObject<TheTvDbApiResponse<SeriesExtendedRecord>>(seriesJson);
            var season = series.Data.Seasons.First(x => x.Number == 1 && x.Name == "Aired Order");
            var seasonJson = await sut.GetSeasonExtendedAsync(season.Id, authToken, null);
            TheTvDbApiResponse<SeasonExtendedRecord> seasonEx =
                JsonConvert.DeserializeObject<TheTvDbApiResponse<SeasonExtendedRecord>>(seasonJson);
            var episode = seasonEx.Data.Episodes.First(x => x.Number == 1);


            // Act
            var actual = await sut.GetEpisodeTranslationAsync(episode.Id, "eng", authToken, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            TheTvDbApiResponse<Translation> response =
                JsonConvert.DeserializeObject<TheTvDbApiResponse<Translation>>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Status, Is.EqualTo("success"));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Overview, Is.Not.Null);
            Assert.That(response.Data.Overview, Is.Not.Empty);
        }
    }
}
