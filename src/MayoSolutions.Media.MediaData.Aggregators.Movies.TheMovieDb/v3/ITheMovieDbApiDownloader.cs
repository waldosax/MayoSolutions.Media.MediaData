using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Movies;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies.TheMovieDb.v3
{
    public interface ITheMovieDbApiDownloader
    {
        //Task<string> GetAuthTokenAsync(IWebProxy proxy);

        Task<string> FindByExternalId(MovieSearchParams searchParams, IWebProxy proxy);

        Task<string> GetMovieSearchResultsAsync(MovieSearchParams searchParams, IWebProxy proxy);
        Task<string> GetMovieSearchResultsAsync(MovieSearchParams searchParams, int page, IWebProxy proxy);

        Task<string> GetMovieDetailsAsync(IMovieIdentifier movieIdentifier, IWebProxy proxy);

    }
}