using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.TV;
using IWebProxy = MayoSolutions.Framework.Web.IWebProxy;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4
{
    public class TheTvDbApiDownloaderV4 : ITheTvDbApiDownloaderV4
    {
        private class Endpoints
        {
            public const string Login = "/login";

            public const string SeriesSearch = "/search";

            public const string GetSeries = "/series/{0}/extended";

            public const string GetSeason = "/seasons/{0}/extended";

            public const string GetEpisodeTranslation = "/episodes/{0}/translations/{1}";
        }


        private readonly ITheTvDbConfigurationValuesV4 _configurationValues;
        private readonly IHttpDownloader _httpDownloader;

        public TheTvDbApiDownloaderV4(
            ITheTvDbConfigurationValuesV4 configurationValues,
            IHttpDownloader httpDownloader
        )
        {
            _configurationValues = configurationValues;
            _httpDownloader = httpDownloader;
        }

        public async Task<string> GetAuthTokenAsync(IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string apiKey = _configurationValues.ApiKey;
            string pin = _configurationValues.ApiPin;

            string url = baseUrl + Endpoints.Login;
            string body = $"{{\"apiKey\": \"{apiKey}\", \"pin\": \"{pin}\"}}";
            string json = await _httpDownloader.PostStringAsync(url, body,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Content-Type", "application/json"}
                },
                proxy);
            return json;
        }

        public async Task<string> GetSeriesSearchResultsAsync(SeriesSearchParams searchParams, string authToken, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string url = baseUrl + Endpoints.SeriesSearch;

            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                {"type", "series"},
            };
            if (!string.IsNullOrWhiteSpace(searchParams?.Name))
                queryParams.Add("query", searchParams.Name);
            // TODO: Search by IMDB ID
            //if (!string.IsNullOrWhiteSpace(searchParams?.ImdbId))
            //    queryParams.Add("imdbId", searchParams.ImdbId);

            string queryString = "?";
            foreach (var param in queryParams)
            {
                if (queryString.Length > 1) queryString += "&";
                queryString += $"{WebUtility.UrlEncode(param.Key)}={WebUtility.UrlEncode(param.Value)}";
            }
            url += queryString;

            string json = await _httpDownloader.GetStringAsync(url,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Authorization", $"Bearer {authToken}"}
                },
                proxy);
            return json;

        }
        

        public async Task<string> GetSeriesExtendedAsync(ISeriesIdentifier seriesIdentifier, string authToken, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string url = baseUrl + string.Format(Endpoints.GetSeries, seriesIdentifier.Id);

            string json = await _httpDownloader.GetStringAsync(url,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Authorization", $"Bearer {authToken}"}
                },
                proxy);
            return json;
        }
        

        public async Task<string> GetSeasonExtendedAsync(long seasonId, string authToken, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string url = baseUrl + string.Format(Endpoints.GetSeason, seasonId);

            string json = await _httpDownloader.GetStringAsync(url,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Authorization", $"Bearer {authToken}"}
                },
                proxy);
            return json;
        }

        public async Task<string> GetEpisodeTranslationAsync(long episodeId, string language, string authToken, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string url = baseUrl + string.Format(Endpoints.GetEpisodeTranslation, episodeId, language);

            string json = await _httpDownloader.GetStringAsync(url,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Authorization", $"Bearer {authToken}"}    // TODO: Extension method
                },
                proxy);
            return json;
        }
    }
}