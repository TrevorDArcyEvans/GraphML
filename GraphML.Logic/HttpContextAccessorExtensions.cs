using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using IdentityModel;

namespace GraphML.Logic
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

		public static bool HasRole(this IHttpContextAccessor context, string role)
		{
			// ClaimTypes.Role --> 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role'
			// JwtClaimTypes.Role --> 'role'
			return context.HttpContext.User.Claims
			    .Where(x => x.Type == ClaimTypes.Role || x.Type == JwtClaimTypes.Role)
			    .Select(x => x.Value)
			    .Any(x => x == role);
		}
	}
}
