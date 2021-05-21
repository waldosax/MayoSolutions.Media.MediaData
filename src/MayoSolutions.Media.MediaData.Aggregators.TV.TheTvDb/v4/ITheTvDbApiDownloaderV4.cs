using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.TV;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4
{
    public interface ITheTvDbApiDownloaderV4
    {
        Task<string> GetAuthTokenAsync(IWebProxy proxy);
        
        Task<string> GetSeriesSearchResultsAsync(SeriesSearchParams searchParams, string authToken, IWebProxy proxy);

        Task<string> GetSeriesExtendedAsync(ISeriesIdentifier seriesIdentifier, string authToken, IWebProxy proxy);

        Task<string> GetSeasonExtendedAsync(long seasonId, string authToken, IWebProxy proxy);

        Task<string> GetEpisodeTranslationAsync(long episodeId, string language, string authToken, IWebProxy proxy);
    }
}
