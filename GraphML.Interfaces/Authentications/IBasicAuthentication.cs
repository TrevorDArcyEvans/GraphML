using System.Threading.Tasks;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace GraphML.Interfaces.Authentications
{
  public interface IBasicAuthentication
  {
    Task Authenticate(ValidatePrincipalContext context);
  }
}
