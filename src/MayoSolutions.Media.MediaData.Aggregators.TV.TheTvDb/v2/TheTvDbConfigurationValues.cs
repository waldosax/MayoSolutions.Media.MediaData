using MayoSolutions.Media.MediaData.Aggregators.Configuration;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2
{
    public class TheTvDbConfigurationValues : ITheTvDbConfigurationValues
    {
        private readonly IConfigurationValues _configurationValues;

        public string ApiBaseUrl => _configurationValues.GetConfigValue("TheTvDb.ApiBaseUrl");
        public string ApiKey => _configurationValues.GetConfigValue("TheTvDb.ApiKey");

        public int AuthTokenCacheDurationInHours
        {
            get
            {
                string cacheDurationString = _configurationValues.GetConfigValue("TheTvDb.AuthTokenCacheDuration");
                if (int.TryParse(cacheDurationString, out int cacheDuration)) return cacheDuration;
                return 24;
            }
        }
        public string ArtworkBaseUrl => _configurationValues.GetConfigValue("TheTvDb.ArtworkBaseUrl");

        public TheTvDbConfigurationValues(IConfigurationValues configurationValues)
        {
            _configurationValues = configurationValues;
        }
    }
}
