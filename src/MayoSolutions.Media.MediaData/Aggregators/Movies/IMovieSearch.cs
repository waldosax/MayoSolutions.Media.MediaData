using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Movies;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies
{
    public interface IMovieSearch
    {
        /// <summary>
        /// Search for a movie using the specified parameters.
        /// </summary>
        /// <param name="searchParams">Search parameters.</param>
        /// <param name="proxy">Optional. Proxy information to use.</param>
        /// <returns>Returns an array of <see cref="Movie" /> search results.</returns>
        Task<Movie[]> SearchAsync(MovieSearchParams searchParams, IWebProxy proxy);
    }
}