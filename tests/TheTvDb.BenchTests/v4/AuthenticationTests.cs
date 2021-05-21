using System;
using System.Threading.Tasks;
using MayoSolutions.Framework;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Tests.Shared.Configuration;

namespace TheTvDb.BenchTests.v4
{
    public class AuthenticationTests
    {
        [Test]
        public async Task LoginShouldReturnToken()
        {
            // Arrange
            var configurationValues = new TheTvDbConfigurationValuesV4(new TestConfigurationValues());
            var sut = new TheTvDbApiDownloaderV4(configurationValues, new HttpDownloader());

            // Act
            var actual = await sut.GetAuthTokenAsync(null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            TheTvDbApiResponse<TheTvDbAuthToken> response =
                JsonConvert.DeserializeObject<TheTvDbApiResponse<TheTvDbAuthToken>>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Status, Is.EqualTo("success"));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Token, Is.Not.Null);
            Assert.That(response.Data.Token, Is.Not.Empty);

            Console.WriteLine(response.Data.Token);
        }

        [Test]
        public async Task AuthenticatorShouldRetrieveAndReturnToken()
        {
            // Arrange
            var configurationValues = new TheTvDbConfigurationValuesV4(new TestConfigurationValues());
            var localPaths = new TestLocalPaths();
            var apiDownloader = new TheTvDbApiDownloaderV4(configurationValues, new HttpDownloader());
            var sut = new TheTvDbAuthenticatorV4(new RealTimeDateProvider(), configurationValues, localPaths, apiDownloader);

            // Act
            var actual = await sut.GetAuthTokenAsync(null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            Console.WriteLine(actual);
        }

    }
}
