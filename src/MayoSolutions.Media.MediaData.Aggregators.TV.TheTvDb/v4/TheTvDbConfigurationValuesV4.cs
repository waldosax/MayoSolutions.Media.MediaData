using MayoSolutions.Media.MediaData.Aggregators.Configuration;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4
{
    public class TheTvDbConfigurationValuesV4 : ITheTvDbConfigurationValuesV4
    {
        private readonly IConfigurationValues _configurationValues;

        public string ApiBaseUrl => _configurationValues.GetConfigValue("TheTvDb.ApiBaseUrl");
        public string ApiKey => _configurationValues.GetConfigValue("TheTvDb.ApiKey");
        public string ApiPin => _configurationValues.GetConfigValue("TheTvDb.ApiPin");

        public int AuthTokenCacheDurationInHours
        {
            get
            {
                string cacheDurationString = _configurationValues.GetConfigValue("TheTvDb.AuthTokenCacheDuration");
                if (int.TryParse(cacheDurationString, out int cacheDuration)) return cacheDuration;
                return 24;
            }
        }

        public TheTvDbConfigurationValuesV4(IConfigurationValues configurationValues)
        {
            _configurationValues = configurationValues;
        }
    }
}
