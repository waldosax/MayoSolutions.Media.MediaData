using System.Threading.Tasks;
using MayoSolutions.Framework.Web;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v2
{
    public interface ITheTvDbAuthenticator
    {
        Task<string> GetAuthTokenAsync(IWebProxy proxy);
        Task<string> ExtendTokenAsync(IWebProxy proxy);
    }
}