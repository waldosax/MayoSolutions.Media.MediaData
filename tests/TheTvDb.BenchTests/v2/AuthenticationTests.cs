using System;
using System.Threading.Tasks;
using MayoSolutions.Framework;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Tests.Shared.Configuration;

namespace TheTvDb.BenchTests.v2
{
    public class AuthenticationTests
    {
        [Test]
        public async Task LoginShouldReturnToken()
        {
            // Arrange
            var configurationValues = new TheTvDbConfigurationValues(new TestConfigurationValues());
            var sut = new TheTvDbApiDownloader(configurationValues, new HttpDownloader());

            // Act
            var actual = await sut.GetAuthTokenAsync(null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            TheTvDbAuthToken response =
                JsonConvert.DeserializeObject<TheTvDbAuthToken>(actual);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Token, Is.Not.Null);
            Assert.That(response.Token, Is.Not.Empty);

            Console.WriteLine(response.Token);
        }

        [Test]
        public async Task AuthenticatorShouldRetrieveAndReturnToken()
        {
            // Arrange
            var configurationValues = new TheTvDbConfigurationValues(new TestConfigurationValues());
            var localPaths = new TestLocalPaths();
            var apiDownloader = new TheTvDbApiDownloader(configurationValues, new HttpDownloader());
            var sut = new TheTvDbAuthenticator(new RealTimeDateProvider(), configurationValues, localPaths, apiDownloader);

            // Act
            var actual = await sut.GetAuthTokenAsync(null);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);

            Console.WriteLine(actual);
        }

    }
}
