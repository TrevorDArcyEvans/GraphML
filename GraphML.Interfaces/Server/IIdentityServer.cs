using System.Threading.Tasks;
using GraphML.Common;

namespace GraphML.Interfaces.Server
{
  public interface IIdentityServer
  {
    Task<LookupEx<string, string>> GetAPIUserClaims();
  }
}
