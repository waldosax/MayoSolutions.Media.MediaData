using System.Threading.Tasks;
using MayoSolutions.Framework.Web;
using MayoSolutions.Media.MediaData.Movies;

namespace MayoSolutions.Media.MediaData.Aggregators.Movies
{
    public interface IMovieDetails
    {
        Task<Movie> GetMovieDetailsAsync(IMovieIdentifier movieIdentifier, IWebProxy proxy);
    }
}