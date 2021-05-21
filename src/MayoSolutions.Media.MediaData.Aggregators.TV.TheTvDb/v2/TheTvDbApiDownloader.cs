using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.TV;
using IWebProxy = MayoSolutions.Framework.Web.IWebProxy;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2
{
    public class TheTvDbApiDownloader : ITheTvDbApiDownloader
    {
        private class Endpoints
        {
            public const string Login = "/login";
            public const string RefreshToken = "/refresh_token";
            public const string SeriesSearch = "/search/series";
            public const string GetSeries = "/series/{0}";
            public const string GetEpisodes = "/series/{0}/episodes";
            public const string GetImageCounts = "/series/{0}/images";
            public const string GetImages = "/series/{0}/images/query";
        }

        private readonly ITheTvDbConfigurationValues _configurationValues;
        private readonly IHttpDownloader _httpDownloader;

        public TheTvDbApiDownloader(
            ITheTvDbConfigurationValues configurationValues,
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

            string url = baseUrl + Endpoints.Login;
            string body = $"{{\"apiKey\": \"{apiKey}\"}}";
            string json = await _httpDownloader.PostStringAsync(url, body,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Content-Type", "application/json"}
                },
                proxy);
            return json;
        }

        public async Task<string> RefreshTokenAsync(string authToken, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string url = baseUrl + Endpoints.RefreshToken;

            string json = await _httpDownloader.GetStringAsync(url,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Content-Type", "application/json"},
                    {"Authorization", $"Bearer {authToken}"}
                },
                proxy);
            return json;
        }

        public async Task<string> GetSeriesSearchResultsAsync(SeriesSearchParams searchParams, string authToken, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string url = baseUrl + Endpoints.SeriesSearch;

            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(searchParams?.Name))
                queryParams.Add("name", searchParams.Name);
            if (!string.IsNullOrWhiteSpace(searchParams?.ImdbId))
                queryParams.Add("imdbId", searchParams.ImdbId);

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

        public async Task<string> GetSeriesAsync(ISeriesIdentifier seriesIdentifier, string authToken, IWebProxy proxy)
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

        public async Task<string> GetSeriesImagesAsync(ISeriesIdentifier seriesIdentifier, string authToken, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string url = baseUrl + string.Format(Endpoints.GetImages, seriesIdentifier.Id);

            List<string> keyTypes = new List<string> { "poster", "fanart", "series", "season", "seasonwide" };
            Dictionary<string, Task<string>> imageTasks = new Dictionary<string, Task<string>>();

            foreach (string keyType in keyTypes)
            {
                string queryUrl = url + $"?keyType={keyType}";
                Task<string> task = _httpDownloader.GetStringAsync(queryUrl,
                    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Authorization", $"Bearer {authToken}"}
                    },
                    proxy);
                imageTasks.Add(keyType, task);
            }

            await Task.WhenAll(imageTasks.Values.Cast<Task>().ToArray());

            string json = "{";
            for (int i = 0; i < keyTypes.Count; i++)
            {
                string keyType = keyTypes[i];
                string terminator = (i < (keyTypes.Count - 1)) ? "," : string.Empty;
                json += $"\t\"{keyType}\":{imageTasks[keyType].Result}{terminator}\r\n";
            }
            json += "}";
            return json;
        }

        public async Task<string> GetEpisodesAsync(ISeriesIdentifier seriesIdentifier, string authToken, IWebProxy proxy)
        {
            return await GetEpisodesAsync(seriesIdentifier, 1, authToken, proxy);
        }

        public async Task<string> GetEpisodesAsync(ISeriesIdentifier seriesIdentifier, int page, string authToken, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string url = baseUrl + string.Format(Endpoints.GetEpisodes, seriesIdentifier.Id);
            url += $"?page={page}";

            string json = await _httpDownloader.GetStringAsync(url,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    {"Authorization", $"Bearer {authToken}"}
                },
                proxy);
            return json;
        }
    }
}