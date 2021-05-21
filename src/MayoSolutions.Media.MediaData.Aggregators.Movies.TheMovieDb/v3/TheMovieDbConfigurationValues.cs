using MayoSolutions.Media.MediaData.Aggregators.Configuration;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3
{
    public class TheMovieDbConfigurationValues : ITheMovieDbConfigurationValues
    {
        private readonly IConfigurationValues _configurationValues;

        public string ApiBaseUrl => _configurationValues.GetConfigValue("TheMovieDb.v3.ApiBaseUrl");
        public string ApiKey => _configurationValues.GetConfigValue("TheMovieDb.v3.ApiKey");

        public int AuthTokenCacheDurationInHours
        {
            get
            {
                string cacheDurationString = _configurationValues.GetConfigValue("TheMovieDb.v3.AuthTokenCacheDuration");
                if (int.TryParse(cacheDurationString, out int cacheDuration)) return cacheDuration;
                return 24;
            }
        }

        public TheMovieDbConfigurationValues(IConfigurationValues configurationValues)
        {
            _configurationValues = configurationValues;
        }
    }
}
