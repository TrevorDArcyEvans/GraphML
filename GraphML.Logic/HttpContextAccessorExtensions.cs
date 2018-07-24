using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace GraphML.Logic
{
  public static class HttpContextAccessorExtensions
  {
    public static string ContextOrganisationId(this IHttpContextAccessor context)
    {
      return context.HttpContext.User.Claims
        .FirstOrDefault(x => x.Type == "Organisation")?.Value;
    }

    public static bool HasRole(this IHttpContextAccessor context, string role)
    {
      return context.HttpContext.User.Claims
        .Where(x => x.Type == ClaimTypes.Role)
        .Select(x => x.Value)
        .Any(x => x == role);
    }
  }
}
