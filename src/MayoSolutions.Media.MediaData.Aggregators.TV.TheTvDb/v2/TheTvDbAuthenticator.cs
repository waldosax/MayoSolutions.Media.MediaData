using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MayoSolutions.Framework;
using MayoSolutions.Framework.Configuration;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2.Models;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2
{
    public class TheTvDbAuthenticator : ITheTvDbAuthenticator
    {
        private const string PersistentAuthTokenFileName = "thetvdb.com.authtoken.json";

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITheTvDbConfigurationValues _configurationValues;
        private readonly ILocalPaths _localPaths;
        private readonly ITheTvDbApiDownloader _apiDownloader;

        private static readonly object _cachedAuthTokenSyncObject = new object();
        private string _cachedAuthToken;
        private DateTime? _tokenExpirationDate;

        public TheTvDbAuthenticator(
            IDateTimeProvider dateTimeProvider,
            ITheTvDbConfigurationValues configurationValues,
            ILocalPaths localPaths,
            ITheTvDbApiDownloader apiDownloader
            )
        {
            _dateTimeProvider = dateTimeProvider;
            _configurationValues = configurationValues;
            _localPaths = localPaths;
            _apiDownloader = apiDownloader;
        }

        public async Task<string> GetAuthTokenAsync(IWebProxy proxy)
        {
            bool getFreshToken = NeedsFreshToken();
            if (!getFreshToken) return _cachedAuthToken;

            RestoreToken();
            getFreshToken = NeedsFreshToken();
            if (!getFreshToken) return _cachedAuthToken;

            string authTokenJson = await _apiDownloader.GetAuthTokenAsync(proxy);
            var authToken = DeserializeTokenResponse(authTokenJson);
            SetToken(authToken.Token);

            return await ExtendTokenAsync(proxy);
        }

        private static TheTvDbAuthToken DeserializeTokenResponse(string authTokenJson)
        {
            var authToken = JsonConvert.DeserializeObject<TheTvDbAuthToken>(authTokenJson);
            return authToken;
        }

        private void RestoreToken()
        {
            string tokenFileName = Path.Combine(_localPaths.ApplicationDataPath, PersistentAuthTokenFileName);
            if (!File.Exists(tokenFileName)) return;

            try
            {
                string json = File.ReadAllText(tokenFileName);
                Dictionary<string, string> authToken = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                authToken = new Dictionary<string, string>(authToken, StringComparer.OrdinalIgnoreCase);
                string token = authToken["token"];
                string expires = authToken["expires"];
                if (DateTime.TryParse(expires, out DateTime expiration))
                    SetToken(token, expiration, false);
            }
            catch (Exception)
            {
            }
        }

        private void PersistToken(string authToken, DateTime expiration)
        {
            Directory.CreateDirectory(_localPaths.ApplicationDataPath);
            string tokenFileName = Path.Combine(_localPaths.ApplicationDataPath, PersistentAuthTokenFileName);
            string json = $"{{\"token\":\"{authToken}\", \"expires\":\"{expiration.ToUniversalTime():R}\"}}";
            File.WriteAllText(tokenFileName, json);
        }

        private void SetToken(string authToken)
        {
            SetToken(authToken, _dateTimeProvider.Now.AddHours(_configurationValues.AuthTokenCacheDurationInHours));
        }
        private void SetToken(string authToken, DateTime expiration, bool persist = true)
        {
            lock (_cachedAuthTokenSyncObject)
            {
                _cachedAuthToken = authToken;
                _tokenExpirationDate = expiration;
                if (persist) PersistToken(authToken, expiration);
            }
        }

        public async Task<string> ExtendTokenAsync(IWebProxy proxy)
        {
            if (NeedsFreshToken()) return await GetAuthTokenAsync(proxy).ConfigureAwait(false);

            string authTokenJson = await _apiDownloader.RefreshTokenAsync(_cachedAuthToken, proxy);
            var authToken = DeserializeTokenResponse(authTokenJson);
            SetToken(authToken.Token);

            return authToken.Token;
        }

        private bool NeedsFreshToken() => !IsInitialized() || IsExpired();
        private bool IsInitialized() => !string.IsNullOrEmpty(_cachedAuthToken);
        private bool IsExpired()
        {
            if (_tokenExpirationDate == null) return false;
            DateTime now = _dateTimeProvider.Now;
            return now >= _tokenExpirationDate.Value;
        }
    }
}
