using MayoSolutions.Framework.Configuration;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3
{
    public class TheMovieDbConfigurationValues : ITheMovieDbConfigurationValues
    {
        private readonly IConfigurationValues _configurationValues;

        public string ApiBaseUrl => _configurationValues.GetConfigValue("TheMovieDb.v3.ApiBaseUrl");
        public string ApiKey => _configurationValues.GetConfigValue("TheMovieDb.v3.ApiKey");
        public string ArtworkBaseUrl => _configurationValues.GetConfigValue("TheMovieDb.v3.ArtworkBaseUrl");

        public TheMovieDbConfigurationValues(IConfigurationValues configurationValues)
        {
            _configurationValues = configurationValues;
        }
    }
}
