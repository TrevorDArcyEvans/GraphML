using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;

namespace GraphML.Common
{
  public static class HttpContextAccessorExtensions
  {
    public static string Email(this IHttpContextAccessor context)
    {
      // ClaimTypes.Email --> 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
      // JwtClaimTypes.Email --> email'
      return context.HttpContext.User.Claims
        .FirstOrDefault(x => x.Type == ClaimTypes.Email || x.Type == JwtClaimTypes.Email)
        ?.Value;
    }
  }
}
