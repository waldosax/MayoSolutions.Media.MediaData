using System.Threading.Tasks;
using MayoSolutions.Framework.Web;

namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4
{
    public interface ITheTvDbAuthenticatorV4
    {
        Task<string> GetAuthTokenAsync(IWebProxy proxy);
    }
}