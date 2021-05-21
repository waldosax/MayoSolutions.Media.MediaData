using MayoSolutions.Common.Extensions;
using MayoSolutions.Media.MediaData.Aggregators.Configuration;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4
{
    public class TheTvDbConfigurationValuesV4 : ITheTvDbConfigurationValuesV4
    {
        private readonly IConfigurationValues _configurationValues;

        public string ApiBaseUrl => _configurationValues.GetConfigValue("TheTvDb.v4.ApiBaseUrl");
        public string ApiKey => _configurationValues.GetConfigValue("TheTvDb.v4.ApiKey");
        public string ApiPin => _configurationValues.GetConfigValue("TheTvDb.v4.Pin");

        public int AuthTokenCacheDurationInHours =>
            _configurationValues.GetConfigValue("TheTvDb.v4.AuthTokenCacheDuration")
                .ToNullableInteger() ?? 24;

        public TheTvDbConfigurationValuesV4(IConfigurationValues configurationValues)
        {
            _configurationValues = configurationValues;
        }
    }
}
