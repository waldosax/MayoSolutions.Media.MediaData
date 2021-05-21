using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.TV;

namespace MayoSolutions.Media.MediaData.Aggregators.TV
{
    /// <summary>
    /// Episode resolver interface.
    /// </summary>
    public interface IEpisodeResolver
    {
        Task<Series> GetEpisodesAsync(ISeriesIdentifier seriesIdentifier, IWebProxy proxy);
    }
}