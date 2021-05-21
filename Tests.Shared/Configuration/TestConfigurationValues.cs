using System;
using System.Collections.Generic;
using MayoSolutions.Media.MediaData.Aggregators.Configuration;

namespace Tests.Shared.Configuration
{
    public class TestConfigurationValues : IConfigurationValues
    {
        private Dictionary<string, string> _values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "TheTvDb.ApiBaseUrl", "https://api.thetvdb.com"},
            {"TheTvDb.ApiKey", "9e717ffe7f88a75466153ee5f714ef5a"},
            {"TheTvDb.AuthTokenCacheDuration", "24"},
            {"TheTvDb.ArtworkBaseUrl", "https://artworks.thetvdb.com/banners"},
            {"TheTvDb.v4.ApiBaseUrl", "https://api4.thetvdb.com/v4"},
            {"TheTvDb.v4.ApiKey", "00446b4a-17a6-4573-9c29-3461bef21d9d"},
            {"TheTvDb.v4.Pin", "UH9KAVPC"},
            {"TheTvDb.v4.AuthTokenCacheDuration", "24"},
            {"TheMovieDb.v3.ApiBaseUrl", "https://api.themoviedb.org/3"},
            {"TheMovieDb.v3.ApiKey", "ce15740ed80a7daf1c1384becaa2b9a5"},
            {"TheMovieDb.v3.ArtworkBaseUrl", "https://www.themoviedb.org/t/p/w600_and_h900_bestv2"},
        };

        public string GetConfigValue(string key)
        {
            if (!_values.ContainsKey(key)) return null;
            return _values[key];
        }
    }
}
