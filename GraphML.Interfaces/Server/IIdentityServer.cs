using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
    public interface IIdentityServer
    {
        Task<string> GetAPIUserClaimsJson();
    }
}
