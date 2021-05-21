using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2.Models;
using MayoSolutions.Media.MediaData.TV;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2
{
    [Aggregator("TheTVDB.com (Legacy V2)")]
    public class TheTvDbEpisodeDataProvider : IAggregator
    {
        private const int MaxSearchResults = 10;
        private const int MaxImageResults = 5;
        private readonly ITheTvDbConfigurationValues _configurationValues;
        private readonly ITheTvDbAuthenticator _authenticator;
        private readonly ITheTvDbApiDownloader _apiDownloader;

        public TheTvDbEpisodeDataProvider(
            ITheTvDbConfigurationValues configurationValues,
            ITheTvDbAuthenticator authenticator,
            ITheTvDbApiDownloader apiDownloader
        )
        {
            _configurationValues = configurationValues;
            _authenticator = authenticator;
            _apiDownloader = apiDownloader;
        }


        public virtual async Task<Series[]> SearchAsync(SeriesSearchParams searchParams,
            IWebProxy proxyPreferences)
        {
            string authToken = await _authenticator.GetAuthTokenAsync(proxyPreferences);

            string json = await _apiDownloader.GetSeriesSearchResultsAsync(searchParams, authToken, proxyPreferences);
            TheTvDbSeriesSearchResult searchResults = JsonConvert.DeserializeObject<TheTvDbSeriesSearchResult>(json);

            TheTvDbSeries[] data = searchResults.Data?.Take(MaxSearchResults).ToArray();

            Dictionary<string, Task<string>> imagesTasks =
                    data?
                        .Select(x => x.Id)
                        .ToDictionary(
                            x => x.ToString(),
                            x => _apiDownloader
                                .GetSeriesImagesAsync(
                                    new SeriesIdentifier {Id = x.ToString()},
                                    authToken, proxyPreferences))
                ;

            if (imagesTasks != null)
                await Task.WhenAll(imagesTasks.Values.ToArray());

            List<Series> seriesResults = data?
                .Select(x => Adapt(
                    x,
                    JsonConvert.DeserializeObject<TheTvDbSeriesImageResults>(imagesTasks[x.Id.ToString()]?.Result)))
                .ToList();
            return seriesResults?.ToArray() ?? new Series[0];
        }

        public virtual async Task<Series> GetEpisodesAsync(ISeriesIdentifier seriesIdentifier,
            IWebProxy proxyPreferences)
        {
            string authToken = await _authenticator.GetAuthTokenAsync(proxyPreferences);

            Task<Series> seriesTask = GetSeriesInfo(seriesIdentifier, authToken, proxyPreferences);
            Task<List<Season>> seasonsTask = GetAllEpisodesInfo(seriesIdentifier, authToken, proxyPreferences);

            Task.WaitAll(seriesTask, seasonsTask);

            Series series = seriesTask.Result;
            if (string.IsNullOrWhiteSpace(series.Name) &&
                !string.IsNullOrWhiteSpace(seriesIdentifier.Name))
            {
                series.Name = seriesIdentifier.Name;
            }
            List<Season> seasons = seasonsTask.Result;

            seasons.ForEach(x => x.Series = series);
            seasons.SelectMany(x => x.Episodes).ToList().ForEach(x => x.Series = series);
            series.Seasons.AddRange(seasons);
            return series;
        }

        private async Task<List<Season>> GetAllEpisodesInfo(ISeriesIdentifier seriesIdentifier, string authToken,
            IWebProxy proxyPreferences)
        {
            List<TheTvDbEpisode> theTvDbEpisodes = new List<TheTvDbEpisode>();
            int pageNumber = 1;
            while (true)
            {
                string episodesJson =
                    await _apiDownloader.GetEpisodesAsync(seriesIdentifier, pageNumber, authToken, proxyPreferences);
                TheTvDbEpisodeResult episodesResults =
                    JsonConvert.DeserializeObject<TheTvDbEpisodeResult>(episodesJson);
                if (episodesResults?.Data == null) break;
                theTvDbEpisodes.AddRange(episodesResults.Data);
                if (episodesResults.Links?.Next == null) break;
                pageNumber = episodesResults.Links.Next.Value;
            }

            List<Season> seasons = Adapt(theTvDbEpisodes).ToList();
            return seasons;
        }

        private async Task<Series> GetSeriesInfo(ISeriesIdentifier seriesIdentifier, string authToken,
            IWebProxy proxyPreferences)
        {
            Task<string> getSeriesTask = _apiDownloader.GetSeriesAsync(seriesIdentifier, authToken, proxyPreferences);
            Task<string> getSeriesImagesTask =
                _apiDownloader.GetSeriesImagesAsync(seriesIdentifier, authToken, proxyPreferences);

            await Task.WhenAll(getSeriesTask, getSeriesImagesTask);

            string seriesJson = getSeriesTask.Result;
            TheTvDbSeriesResult seriesResults = JsonConvert.DeserializeObject<TheTvDbSeriesResult>(seriesJson);

            string imagesJson = getSeriesImagesTask.Result;
            TheTvDbSeriesImageResults imageResults =
                JsonConvert.DeserializeObject<TheTvDbSeriesImageResults>(imagesJson);

            Series series = Adapt(seriesResults.Data, imageResults);
            return series;
        }


        private Series Adapt(TheTvDbSeries theTvDbSeries, TheTvDbSeriesImageResults theTvDbSeriesImages)
        {
            Adapt(theTvDbSeries, out Series series);
            Adapt(theTvDbSeries, theTvDbSeriesImages, out SeriesImageUrls seriesMetadata);
            series.ImageUrls = seriesMetadata;
            return series;
        }

        private void Adapt(TheTvDbSeries theTvDbSeries, TheTvDbSeriesImageResults images, out SeriesImageUrls output)
        {
            output = new SeriesImageUrls();
            if (images == null) return;

            output.PosterImageUrls = images.Poster?.Data?.Take(MaxImageResults)
                .Select(x => _configurationValues.ArtworkBaseUrl + "/" + x.FileName)
                .ToArray();
            output.BackgroundImageUrls = images.FanArt?.Data?.Take(MaxImageResults)
                .Select(x => _configurationValues.ArtworkBaseUrl + "/" + x.FileName)
                .ToArray();
            output.BannerImageUrls =
                ((!string.IsNullOrWhiteSpace(theTvDbSeries.Banner) ? new [] {theTvDbSeries.Banner} : new string[0])
                .Concat(images.Series?.Data?.Select(x => x.FileName) ?? new string[0]))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(MaxImageResults)
                .Select(x => _configurationValues.ArtworkBaseUrl + "/" + x)
                .ToArray();
        }

        private IEnumerable<Season> Adapt(IEnumerable<TheTvDbEpisode> theTvDbEpisodes)
        {
            List<Season> seasons = new List<Season>();
            var groupedBySeasonNumber = theTvDbEpisodes.GroupBy(x => x.AiredSeason).OrderBy(x => x.Key);
            foreach (var episodesInSeason in groupedBySeasonNumber)
            {
                Adapt(episodesInSeason.First(), out Season season);
                seasons.Add(season);
                foreach (var theTvDbEpisode in episodesInSeason.OrderBy(x => x.AiredEpisodeNumber))
                {
                    Adapt(theTvDbEpisode, out Episode episode);
                    episode.Season = season;
                    season.Episodes.Add(episode);
                }
            }

            return seasons;
        }
        
        private void Adapt(TheTvDbSeries input, out Series output)
        {
            output = new Series
            {
                Name = input.SeriesName,
                Id = input.SeriesId,
                ImdbId = input.ImdbId,
                AirDate = input.AirDate,
                Genres = input.Genre,
                Networks = new []{ input.Network },
                Status = input.Status,
                Description = input?.Overview,
                Year = input?.AirDate?.Year,
                // TODO: FIX ImageUrls = {PosterImageUrls = input.ImageUrls.PosterImageUrls}
            };
        }

        private void Adapt(IEpisodeDescriptor input, out Season output)
        {
            output = new Season
            {
                SeasonNumber = input.SeasonNumber
            };
        }

        private void Adapt(IEpisodeDescriptor input, out Episode output)
        {
            output = new Episode
            {
                EpisodeNumber = input.EpisodeNumber,
                Title = input.Title,
                Description = input.Description,
                AirDate = input.AirDate,
                AdditionalData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            };
            JsonConvert.PopulateObject(JsonConvert.SerializeObject(input), output.AdditionalData);
        }
    }
}
