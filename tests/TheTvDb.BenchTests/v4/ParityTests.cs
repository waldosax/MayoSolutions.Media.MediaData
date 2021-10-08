using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayoSolutions.Common.EditDistance;
using MayoSolutions.Common.Extensions;
using MayoSolutions.Framework;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.TV;
using MayoSolutions.Media.MediaData.TV;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Tests.Shared.Configuration;
using Tests.Shared.TheTvDb;
using TheTvDb.BenchTests.v4;
using V2 = MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2;
using V4 = MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4;

namespace MayoSolutions.NameMyTvSeries.Core.BenchTests.Aggregators.TheTvDb.v4
{
    public class ParityTests
    {
        private static object[][] TestCases()
        {
            return ((KnownTvShowIds[])Enum.GetValues(typeof(KnownTvShowIds)))
                .Select(e => new object[] { e }).ToArray();
        }

        private static string GetSearchTerm(KnownTvShowIds showId)
        {
            switch (showId)
            {
                case KnownTvShowIds.AgentsOfSHIELD: return "Marvel's Agents of S.H.I.E.L.D.";
                case KnownTvShowIds.ChappellesShow: return "Chappelle's Show";
                case KnownTvShowIds.SchittsCreek: return "Schitt's Creek";
                case KnownTvShowIds.WuTangAnAmericanSaga: return "Wu Tang : An American Saga";  // NOTE: Might have to scrub data before putting it in search (replace dash with space)
                case KnownTvShowIds.Blackish: return "Black ish";  // NOTE: Might have to scrub data before putting it in search (replace dash with space)
                default: return showId.FolderName();
            }
        }

        protected ISeriesIdentifier GetSeriesIdentifier(KnownTvShowIds tvdbShowId)
        {
            return new SeriesIdentifier { Id = tvdbShowId.ToString("D") };
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async Task AreDataProvidersComparable(KnownTvShowIds showId)
        {
            // Arrange
            var searchTerm = GetSearchTerm(showId);
            var dateProvider = new RealTimeDateProvider();
            var httpDownloader = new HttpDownloader();
            var configValues = new TestConfigurationValues();
            var localPaths = new TestLocalPaths();

            var v2Config = new V2.TheTvDbConfigurationValues(configValues);
            var v2ApiDownloader = new V2.TheTvDbApiDownloader(v2Config, httpDownloader);
            var v2Authenticator = new V2.TheTvDbAuthenticator(dateProvider, v2Config, localPaths, v2ApiDownloader);
            var v2DataProvider = new V2.TheTvDbEpisodeDataProvider(v2Config, v2Authenticator, v2ApiDownloader);

            var v4Config = new V4.TheTvDbConfigurationValuesV4(configValues);
            var v4ApiDownloader = new V4.TheTvDbApiDownloaderV4(v4Config, httpDownloader);
            var v4Authenticator = new V4.TheTvDbAuthenticatorV4(dateProvider, v4Config, localPaths, v4ApiDownloader);
            var v4DataProvider = new V4.TheTvDbEpisodeDataProviderV4(v4Authenticator, v4ApiDownloader);

            var searchParams = new SeriesSearchParams { Name = searchTerm };

            // Act
            var v2SearchResult = await v2DataProvider.SearchAsync(searchParams, null);
            var v4SearchResult = await v4DataProvider.SearchAsync(searchParams, null);

            // Assert
            Assert.That(v2SearchResult, Is.Not.Null, "Expected v2 search results to be not null.");
            Assert.That(v4SearchResult, Is.Not.Null, "Expected v4 search results to be not null.");

            Assert.That(v2SearchResult.Length, Is.GreaterThan(0), "Expected to have v2 search results.");
            Assert.That(v4SearchResult.Length, Is.GreaterThan(0), "Expected to have v4 search results.");

            var v2BestMatch = v2SearchResult.FirstOrDefault(x => x.Id == showId.ToString("D"));

            Assert.That(v2BestMatch, Is.Not.Null, "Expected best matching v2 search result to be not null.");
            Assert.That(v2BestMatch.Id, Is.EqualTo(showId.ToString("D")), "Expected best matching v2 search result to be {0}, but was {1}.", searchTerm, v2BestMatch.Name);

            var v4BestMatch = v4SearchResult.FirstOrDefault(x => x.Id == v2BestMatch.Id);
            Assert.That(v4BestMatch, Is.Not.Null, "Expected equivalent v4 search result.");

            Assert.That(v4BestMatch.Name, Is.EqualTo(v2BestMatch.Name), "Expected names to match.");
            //Assert.That(v4BestMatch.SeriesIdentifier.ImdbId, Is.EqualTo(v2BestMatch.SeriesIdentifier.ImdbId), "Expected IMDB IDs to match.");
            Assert.That(v4BestMatch.Year, Is.EqualTo(v2BestMatch.Year), "Expected years to match.");
            //Assert.That(v4BestMatch.Description, Is.EqualTo(v2BestMatch.Description), "Expected descriptions to match.");

            var seriesIdentifier = GetSeriesIdentifier(showId);
            var v2Series = await v2DataProvider.GetEpisodesAsync(seriesIdentifier, null);
            var v4Series = await v4DataProvider.GetEpisodesAsync(seriesIdentifier, null);

            ParityDump.Dump(v2Series, v4Series, Console.Out);

            Assert.That(v2Series, Is.Not.Null, "Expected v2 series to be not null.");
            Assert.That(v4Series, Is.Not.Null, "Expected v4 series to be not null.");

            Assert.That(v4Series.Id, Is.EqualTo(v2Series.Id), "Expected series IDs to match.");
            Assert.That(v4Series.Name, Is.EqualTo(v2Series.Name), "Expected series names to match.");
            //Assert.That(v4Series.ImdbId, Is.EqualTo(v2Series.ImdbId), "Expected series IMDB IDs to match.");
            //Assert.That(v4Series.Year, Is.EqualTo(v2Series.Year), "Expected series years to match.");
            //Assert.That(v4Series.AirDate, Is.EqualTo(v2Series.AirDate), "Expected series air dates to match.");
            //Assert.That(v4Series.Network, Is.EqualTo(v2Series.Network), "Expected series networks to match.");
            Assert.That(v4Series.Status, Is.EqualTo(v2Series.Status), "Expected series statuses to match.");


            Assert.That(v2Series.Seasons, Is.Not.Null);
            Assert.That(v4Series.Seasons, Is.Not.Null);
            Assert.That(v4Series.Seasons.Count, Is.EqualTo(v2Series.Seasons.Count), "Expected season counts to match.");
            for (int i = 0; i < v4Series.Seasons.Count; i++)
            {
                var v2Season = v2Series.Seasons[i];
                var v4Season = v4Series.Seasons[i];

                Assert.That(v4Season.SeasonNumber, Is.EqualTo(v2Season.SeasonNumber));

                Assert.That(v2Season.Episodes, Is.Not.Null);
                Assert.That(v4Season.Episodes, Is.Not.Null);
                Assert.That(v4Season.Episodes.Count, Is.EqualTo(v2Season.Episodes.Count), "Expected episode counts to match.");

                for (int j = 0; j < v4Season.Episodes.Count; j++)
                {
                    var v2Episode = v2Season.Episodes[j];
                    var v4Episode = v4Season.Episodes[j];

                    Assert.That(v4Episode.EpisodeNumber, Is.EqualTo(v2Episode.EpisodeNumber));
                    Assert.That(IsCloseEnough(v4Episode.Title, v2Episode.Title), "Expected \"{0}\", but was \"{1}\", Season {2} Episode {3}.", v2Episode.Title, v4Episode.Title, v4Season.SeasonNumber, v4Episode.EpisodeNumber);

                }
            }

        }

        // TODO: Extract to constraints
        private static bool IsCloseEnough(string actual, string expected, decimal tolerance = 0.01m)
        {
            if (actual == expected) return true;
            if (string.IsNullOrWhiteSpace(actual) && string.IsNullOrWhiteSpace(expected)) return true;
            if (string.IsNullOrWhiteSpace(actual)) return false;
            if (string.IsNullOrWhiteSpace(expected)) return false;

            actual = actual.Trim().RemoveDiacritics();
            expected = expected.Trim().RemoveDiacritics();

            if (actual.EqualsCaseInsensitive(expected)) return true;

            actual = actual.ToLower().RemoveNonLettersAndDigits();
            expected = expected.ToLower().RemoveNonLettersAndDigits();
            int distance = actual.LevenshteinDistance(expected);
            if (expected.Length / (decimal)distance <= tolerance) return false;

            return false;
        }

    }
}
