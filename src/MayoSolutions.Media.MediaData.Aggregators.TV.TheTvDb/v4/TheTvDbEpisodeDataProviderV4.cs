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
            return seriesResults?.ToArray() ?? new Series[0];
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


            // Step 1. Query the extended details of a series
            string seriesJson = await _apiDownloader.GetSeriesExtendedAsync(seriesIdentifier, authToken, proxy);
            var theTvDbSeries = Deserialize<SeriesExtendedRecord>(seriesJson);


            // Step 2. Extract all the Aired Order seasons
            var allSeasons = theTvDbSeries.Data.Seasons
                .Where(x => x.Type.Name == "Aired Order")
                .OrderBy(x => x.Number)
                .ToList();


            // Step 3. For each season, query the season individually to get episodes
            var seasonsTasks = allSeasons.ToDictionary(
                x => x.Number,
                x => _apiDownloader.GetSeasonExtendedAsync(x.Id, authToken, proxy
                ));
            await Task.WhenAll(seasonsTasks.Values.ToArray());

            var theTvDbEpisodes = seasonsTasks
                .ToDictionary(
                    x => x.Key,
                    x => Deserialize<SeasonExtendedRecord>(x.Value?.Result).Data.Episodes
                        .Where(y => y.SeasonNumber == x.Key) // Linting bad information
                        .ToList()
                );

            // De-dupe episodes
            var dedupedEpisodes = theTvDbEpisodes
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.GroupBy(
                        x => x.Id,
                        x => x
                        )
                        .Select(x => x.First())
                        .OrderBy(x => x.Number)
                        .ToList()
                    );

            // Remove any seasons without episodes
            var seasonNumbersToRemove = dedupedEpisodes
                .Where(kvp => kvp.Value.Count == 0)
                .Select(kvp => kvp.Key)
                .ToArray();
            foreach (var seasonNumber in seasonNumbersToRemove)
            {
                dedupedEpisodes.Remove(seasonNumber);
                allSeasons.RemoveAt(allSeasons.FindIndex(x => x.Number == seasonNumber));
            }

            // Step 4. For each episode, query the translations individually to get descriptions
            var episodesTasks = dedupedEpisodes.SelectMany(kvp => kvp.Value)
                .ToDictionary(
                    x => x.Id,
                    x => _apiDownloader.GetEpisodeTranslationAsync(x.Id, "eng", authToken, proxy
                    ));
            await Task.WhenAll(episodesTasks.Values.ToArray());
            var overviews = episodesTasks.ToDictionary(
                x => x.Key,
                x => Deserialize<Translation>(x.Value.Result).Data
            );

            var series = Adapt(theTvDbSeries.Data, allSeasons, dedupedEpisodes, overviews);
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
            List<SeasonBaseRecord> theTvDbSeasons,
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
                            AirDate = theTvDbEpisode.Aired.ToTvDbDate(),
                            AdditionalData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                        };
                        JsonConvert.PopulateObject(JsonConvert.SerializeObject(theTvDbEpisode), episode.AdditionalData);
                        if (overviews.ContainsKey(theTvDbEpisode.Id))
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
