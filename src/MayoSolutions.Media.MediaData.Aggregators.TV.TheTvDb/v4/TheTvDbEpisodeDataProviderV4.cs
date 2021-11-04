using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MayoSolutions.Common.Extensions;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4.Models;
using MayoSolutions.Media.MediaData.TV;
using Newtonsoft.Json;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4
{
    /// <summary>
    /// Aggregates data from The TVDB API and converts them into objects used by the library.
    /// </summary>
    [Aggregator("TheTVDB.com")]
    public class TheTvDbEpisodeDataProviderV4 : ITvAggregator
    {
        private const int MaxSearchResults = 10;
        private const int MaxImageResults = 5;
        private readonly ITheTvDbAuthenticatorV4 _authenticator;
        private readonly ITheTvDbApiDownloaderV4 _apiDownloader;

        public TheTvDbEpisodeDataProviderV4(
            ITheTvDbAuthenticatorV4 authenticator,
            ITheTvDbApiDownloaderV4 apiDownloader
        )
        {
            _authenticator = authenticator;
            _apiDownloader = apiDownloader;
        }


        public virtual async Task<Series[]> SearchAsync(SeriesSearchParams searchParams,
            IWebProxy proxy)
        {
            string authToken = await _authenticator.GetAuthTokenAsync(proxy);

            string json = await _apiDownloader.GetSeriesSearchResultsAsync(searchParams, authToken, proxy);
            var searchResults = Deserialize<List<SearchResult>>(json);

            SearchResult[] data = searchResults.Data?.Take(MaxSearchResults).ToArray();

            Dictionary<string, Task<string>> imagesTasks =
                    data?
                        .Select(x => x.TheTvDbId)
                        .ToDictionary(
                            x => x,
                            x => _apiDownloader
                                .GetSeriesExtendedAsync(
                                    new SeriesIdentifier { Id = x },
                                    authToken, proxy))
                ;

            if (imagesTasks != null)
                await Task.WhenAll(imagesTasks.Values.ToArray());

            List<Series> seriesResults = data?
                .Select(x => Adapt(
                    x,
                    Deserialize<SeriesExtendedRecord>(imagesTasks[x.TheTvDbId]?.Result)?.Data))
                .ToList();
            return seriesResults?.ToArray() ?? Array.Empty<Series>();
        }



        public virtual async Task<Series> GetEpisodesAsync(ISeriesIdentifier seriesIdentifier,
            IWebProxy proxy)
        {
            string authToken = await _authenticator.GetAuthTokenAsync(proxy);

            var series = await GetAllEpisodesInfo(seriesIdentifier, authToken, proxy);
            return series;
        }
        

        private async Task<Series> GetAllEpisodesInfo(ISeriesIdentifier seriesIdentifier, string authToken,
            IWebProxy proxy)
        {
            // I will hate TheTvDb forever for how they make you get episodes now.

            string seriesJson = await _apiDownloader.GetEpisodesAsync(seriesIdentifier, "official", "eng", authToken, proxy);
            var theTvDbSeries = Deserialize<SeriesExtendedEpisodesRecord>(seriesJson);

            List<ISeasonNumber> seasons;
            if (theTvDbSeries.Data.Seasons != null)
            {
                seasons = theTvDbSeries.Data.Seasons
                    .Where(s => s.Type?.Type == "official")
                    .OrderBy(s => s.Number)
                    .Cast<ISeasonNumber>()
                    .ToList();
            }
            else
            {
                seasons = theTvDbSeries.Data.Episodes
                    .Select(e => e.SeasonNumber ?? -1)
                    .Distinct()
                    .Select(sn => new SeasonNumberShim(sn))
                    .Cast<ISeasonNumber>()
                    .ToList();
            }

            // Remove any episodes which have multiple entries for season number and episode number
            var dedupedEpisodes = theTvDbSeries.Data.Episodes
                .GroupBy(ep => new {ep.SeasonNumber, ep.Number}, ep => ep)
                .Select(g => g.First())
                .GroupBy(ep => ep.SeasonNumber ?? 0L, ep => ep)
                .OrderBy(s => s.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(ep => ep.Number).ToList());

            // Remove seasons with no episodes
            for (int i = seasons.Count - 1; i >= 0; i--)
            {
                var seasonNumber = seasons[i].Number;
                if (!dedupedEpisodes.ContainsKey(seasonNumber) || dedupedEpisodes[seasonNumber].Count == 0)
                    seasons.RemoveAt(i);
            }

            var series = Adapt(theTvDbSeries.Data,
                seasons,
                dedupedEpisodes,
                (Dictionary<long, Translation>)null);

            return series;
        }

        private async Task<Series> GetSeriesInfo(ISeriesIdentifier seriesIdentifier, string authToken,
            IWebProxy proxy)
        {
            string seriesJson =
                await _apiDownloader.GetSeriesExtendedAsync(seriesIdentifier, authToken, proxy);
            var seriesResponse = Deserialize<SeriesExtendedRecord>(seriesJson);

            Series series = Adapt(seriesResponse.Data);
            return series;
        }

        private TheTvDbApiResponse<T> Deserialize<T>(string json)
        {
            // TODO: Errors and serialization exceptions
            return JsonConvert.DeserializeObject<TheTvDbApiResponse<T>>(json);
        }

        private static class ArtworkType
        {
            public const int Banner = 1;
            public const int Poster = 2;
            public const int Background = 3;
            public const int Icon = 5;
            public const int SeasonPoster = 7;
        }
        private static class CompanyType
        {
            public const int Network = 1;
            public const int ProductionCompany = 3;
        }
        private static class RemoteIdType
        {
            public const int OfficialWebsite = 4;
            public const int Twitter = 6;
            public const int IMDB = 2;
            public const int Instagram = 9;
        }

        #region Adapt Search Results

        private Series Adapt(SearchResult searchResult, SeriesExtendedRecord theTvDbSeries)
        {
            Adapt(searchResult, out Series series);

            if (theTvDbSeries == null) return series;

            Adapt(theTvDbSeries, out SeriesImageUrls seriesMetadata);
            series.ImageUrls = seriesMetadata;

            return series;
        }

        private void Adapt(SearchResult input, out Series output)
        {
            output = new Series
            {
                Id = input.TheTvDbId,
                Name = input.Name,
                Description = input.Overview,
                Year = input.Year.ToNullableInteger(),
            };
        }

        #endregion

        #region Adapt Series

        private Series Adapt(SeriesExtendedRecord theTvDbSeries)
        {
            Adapt(theTvDbSeries, out Series series);

            Adapt(theTvDbSeries, out SeriesImageUrls seriesMetadata);
            series.ImageUrls = seriesMetadata;

            return series;
        }

        private void Adapt(SeriesExtendedRecord input, out Series output)
        {
            output = new Series
            {
                Id = input.Id?.ToString(),
                Name = input.Name,
                ImdbId = input.RemoteIds?.FirstOrDefault(x => x.Type == RemoteIdType.IMDB)?.Id,
                Description = input.Translations?.OverviewTranslations?.FirstOrDefault(x => x.Language == "eng")?.Overview,
                Year = input.FirstAired.ToTvDbDate()?.Year,
                ReleaseDate = input.FirstAired.ToTvDbDate(),
                Genres = input.Genres?.Select(x => x.Name)?.ToArray(),
                Networks = input.Networks?.Select(x => x.Name)?.Concat(
                          input.Companies?.Where(x => x.CompanyType.Id == CompanyType.Network).Select(x => x.Name)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray(),
                Status = input.Status?.Name
            };
        }



        private void Adapt(SeriesExtendedRecord theTvDbSeries, out SeriesImageUrls output)
        {
            output = new SeriesImageUrls();

            output.PosterImageUrls = theTvDbSeries.Artwork?
                .Where(x => x.Type == ArtworkType.Poster)
                .OrderByDescending(x => x.Score)
                .Take(MaxImageResults)
                .Select(x => x.Image)
                .ToArray();
            output.BackgroundImageUrls = theTvDbSeries.Artwork?
                .Where(x => x.Type == ArtworkType.Background)
                .OrderByDescending(x => x.Score)
                .Take(MaxImageResults)
                .Select(x => x.Image)
                .ToArray();
            output.BannerImageUrls = theTvDbSeries.Artwork?
                .Where(x => x.Type == ArtworkType.Banner)
                .OrderByDescending(x => x.Score)
                .Take(MaxImageResults)
                .Select(x => x.Image)
                .ToArray();
        }

        #endregion

        #region Adapt Seasons/Episodes

        private Series Adapt(
            SeriesExtendedRecord theTvDbSeries,
            List<ISeasonNumber> theTvDbSeasons,
            Dictionary<long, List<EpisodeBaseRecord>> theTvDbEpisodes,
            Dictionary<long, Translation> overviews
            )
        {

            var series = Adapt(theTvDbSeries);

            foreach (var theTvDbSeason in theTvDbSeasons)
            {
                Season season = new Season
                {
                    Series = series,
                    SeasonNumber = (int)theTvDbSeason.Number
                };

                if (theTvDbEpisodes.ContainsKey(theTvDbSeason.Number))
                {
                    var theTvDbEpisodesInSeason =
                        theTvDbEpisodes[theTvDbSeason.Number].OrderBy(x => x.Number).ToArray();
                    foreach (var theTvDbEpisode in theTvDbEpisodesInSeason)
                    {
                        Episode episode = new Episode
                        {
                            Series = series,
                            Season = season,
                            EpisodeNumber = (int)theTvDbEpisode.Number,
                            Title = theTvDbEpisode.Name,
                            Description = theTvDbEpisode.Overview,
                            AirDate = theTvDbEpisode.Aired.ToTvDbDate(),
                            AdditionalData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                        };
                        JsonConvert.PopulateObject(JsonConvert.SerializeObject(theTvDbEpisode), episode.AdditionalData);
                        if (string.IsNullOrEmpty(episode.Description) && overviews?.ContainsKey(theTvDbEpisode.Id) == true)
                            episode.Description = overviews[theTvDbEpisode.Id].Overview;
                        season.Episodes.Add(episode);

                    }
                }

                series.Seasons.Add(season);
            }

            return series;
        }



        #endregion
    }
}
