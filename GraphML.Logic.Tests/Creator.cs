using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Tests
{
	public static class Creator
	{
		public static DefaultHttpContext GetContext(
		  string email = "DrKool@KoolOrganisation.org")
		{
			var emailClaim = new Claim(ClaimTypes.Email, email);
			var claimsIdentity = new ClaimsIdentity(new[] { emailClaim });
			var user = new ClaimsPrincipal(new[] { claimsIdentity });
			var ctx = new DefaultHttpContext { User = user };

			return ctx;
		}
	}
}
