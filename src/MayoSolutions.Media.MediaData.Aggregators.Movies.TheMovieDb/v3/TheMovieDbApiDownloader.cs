using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Movies;
using IWebProxy = MayoSolutions.Framework.Web.IWebProxy;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3
{
    public class TheMovieDbApiDownloader : ITheMovieDbApiDownloader
    {
        private class Endpoints
        {
            public const string NewAuthenticationToken = "/authentication/token/new";

            public const string MovieSearch = "/search/movie";

            public const string GetMovieByMovieId = "/movie/{0}";

            public const string FindByExternalId = "/find/{0}";
        }

        private readonly ITheMovieDbConfigurationValues _configurationValues;
        private readonly IHttpDownloader _httpDownloader;

        public TheMovieDbApiDownloader(
            ITheMovieDbConfigurationValues configurationValues,
            IHttpDownloader httpDownloader
        )
        {
            _configurationValues = configurationValues;
            _httpDownloader = httpDownloader;
        }


        //public async Task<string> GetAuthTokenAsync(IWebProxy proxy)
        //{
        //    string baseUrl = _configurationValues.ApiBaseUrl;
        //    string apiKey = _configurationValues.ApiKey;

        //    string url = baseUrl + Endpoints.NewAuthenticationToken;
        //    Dictionary<string, string> queryParams = new Dictionary<string, string>
        //    {
        //        {"api_key", apiKey},
        //    };

        //    string queryString = "?";
        //    foreach (var param in queryParams)
        //    {
        //        if (queryString.Length > 1) queryString += "&";
        //        queryString += $"{WebUtility.UrlEncode(param.Key)}={WebUtility.UrlEncode(param.Value)}";
        //    }
        //    url += queryString;
        //    string json = await _httpDownloader.GetStringAsync(url,
        //        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        //        {
        //            {"Content-Type", "application/json"}
        //        },
        //        proxy);
        //    return json;
        //}

        public async Task<string> FindByExternalId(MovieSearchParams searchParams, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string apiKey = _configurationValues.ApiKey;

            string url = baseUrl;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                {"api_key", apiKey},
            };

            if (!string.IsNullOrWhiteSpace(searchParams.ImdbId))
            {
                url = baseUrl + string.Format(Endpoints.FindByExternalId, searchParams.ImdbId);
                queryParams.Add("external_source", "imdb_id");
            }

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
                    {"Content-Type", "application/json"}
                },
                proxy);
            return json;
        }

        public async Task<string> GetMovieSearchResultsAsync(MovieSearchParams searchParams, IWebProxy proxy)
        {
            return await GetMovieSearchResultsAsync(searchParams, 1, proxy);
        }

        public async Task<string> GetMovieSearchResultsAsync(MovieSearchParams searchParams, int page, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string apiKey = _configurationValues.ApiKey;

            string url = baseUrl + Endpoints.MovieSearch;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                {"query", searchParams.Name},
                {"page", page.ToString()},
                {"include_adult",  searchParams.IncludeAdult.ToString().ToLowerInvariant()},
                {"api_key", apiKey},
            };

            if (searchParams.Year.HasValue) queryParams.Add("year", searchParams.Year.Value.ToString());

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
                    {"Content-Type", "application/json"}
                },
                proxy);
            return json;
        }


        public async Task<string> GetMovieDetailsAsync(IMovieIdentifier movieIdentifier, IWebProxy proxy)
        {
            string baseUrl = _configurationValues.ApiBaseUrl;
            string apiKey = _configurationValues.ApiKey;

            string url = baseUrl + string.Format(Endpoints.GetMovieByMovieId, movieIdentifier.Id);
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                {"api_key", apiKey},
            };

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
                    {"Content-Type", "application/json"}
                },
                proxy);
            return json;
        }
    }
}
