using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.TV;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2
{
    public interface ITheTvDbApiDownloader
    {
        Task<string> GetAuthTokenAsync(IWebProxy proxy);
        Task<string> RefreshTokenAsync(string authToken, IWebProxy proxy);
        Task<string> GetSeriesSearchResultsAsync(SeriesSearchParams searchParams, string authToken, IWebProxy proxy);
        Task<string> GetSeriesAsync(ISeriesIdentifier seriesIdentifier, string authToken, IWebProxy proxy);
        Task<string> GetSeriesImagesAsync(ISeriesIdentifier seriesIdentifier, string authToken, IWebProxy proxy);
        Task<string> GetEpisodesAsync(ISeriesIdentifier seriesIdentifier, string authToken, IWebProxy proxy);
        Task<string> GetEpisodesAsync(ISeriesIdentifier seriesIdentifier, int page, string authToken, IWebProxy proxy);
    }
}