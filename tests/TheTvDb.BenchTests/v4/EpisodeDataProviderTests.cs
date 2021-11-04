using System;
using System.Collections.Generic;
using System.Text;
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
    public class EpisodeDataProviderTests
    {
        protected ITheTvDbApiDownloaderV4 CreateDownloader()
        {
            var configurationValues = new TheTvDbConfigurationValuesV4(new TestConfigurationValues());
            return new TheTvDbApiDownloaderV4(configurationValues, new HttpDownloader());
        }

        protected ITvAggregator CreateEpisodeDataProvider()
        {
            var authenticator = GetAuthenticator();
            var downloader = CreateDownloader();
            return new TheTvDbEpisodeDataProviderV4(authenticator, downloader);
        }

        protected ITheTvDbAuthenticatorV4 GetAuthenticator()
        {
            var configurationValues = new TheTvDbConfigurationValuesV4(new TestConfigurationValues());
            var localPaths = new TestLocalPaths();
            var apiDownloader = CreateDownloader();
            var authenticator =
                new TheTvDbAuthenticatorV4(new RealTimeDateProvider(), configurationValues, localPaths, apiDownloader);
            return authenticator;
        }

        protected async Task<string> GetAuthTokenAsync()
        {
            var authenticator = GetAuthenticator();
            return await authenticator.GetAuthTokenAsync(null);
        }

        protected ISeriesIdentifier GetSeriesIdentifier(KnownTvShowIds tvdbShowId)
        {
            return new SeriesIdentifier { Id = tvdbShowId.ToString("D") };
        }

        [Test]
        public async Task TestGetEpisodes()
        {
            // Arrange
            var seriesIdentifier = GetSeriesIdentifier(KnownTvShowIds.WhatWeDoInTheShadows);
            var sut = CreateEpisodeDataProvider();

            // Act
            var actual = await sut.GetEpisodesAsync(seriesIdentifier, null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            
        }

    }
}
