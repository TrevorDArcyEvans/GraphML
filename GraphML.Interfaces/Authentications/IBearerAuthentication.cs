using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Authentications
{
  public interface IBearerAuthentication
  {
    Task Authenticate(TokenValidatedContext context);
  }
}
