using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.TV;

namespace MayoSolutions.Media.MediaData.Aggregators.TV
{
    /// <summary>
    /// Series search interface.
    /// </summary>
    public interface ISeriesSearch
    {
        /// <summary>
        /// Search for a series using the specified parameters.
        /// </summary>
        /// <param name="searchParams">Search parameters.</param>
        /// <param name="proxy">Optional. Proxy information to use.</param>
        /// <returns>Returns an array of <see cref="Series" /> search results.</returns>
        Task<Series[]> SearchAsync(SeriesSearchParams searchParams, IWebProxy proxy);
    }
}