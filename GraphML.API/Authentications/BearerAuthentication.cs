using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphML.API.Authentications
{
  internal static class BearerAuthentication
  {
    private static TimeSpan Expiry = TimeSpan.FromMinutes(60);

    // [bearerToken]-->[UserInfoResponse]
    private static Dictionary<string, CachedUserInfoResponse> _cache = new Dictionary<string, CachedUserInfoResponse>();

    public static async Task Authenticate(IConfiguration config, TokenValidatedContext context)
    {
      // set roles based on email-->organisation-->org.PrimaryRoleId
      var bearerToken = ((FrameRequestHeaders)context.HttpContext.Request.Headers).HeaderAuthorization.Single();

      // have to cache responses or UserInfo endpoint thinks we are a DOS attack
      if (_cache.TryGetValue(bearerToken, out CachedUserInfoResponse cachedresponse))
      {
        if (cachedresponse.Created < DateTime.UtcNow.Subtract(Expiry))
        {
          _cache.Remove(bearerToken);
          cachedresponse = null;
        }
      }

      var response = cachedresponse?.UserInfoResponse;
      if (response == null)
      {
        var userInfoClient = new UserInfoClient(config["Jwt:UserInfo"]);
        response = await userInfoClient.GetAsync(bearerToken.Substring(7));
        _cache.Add(bearerToken, new CachedUserInfoResponse(response));
      }

      var userClaims = response.Claims;
      var claims = new List<Claim>(userClaims);
      var email = userClaims.SingleOrDefault(x => x.Type == "email")?.Value;
      if (!string.IsNullOrEmpty(email))
      {
        var servProv = context.HttpContext.RequestServices;
        //var contLog = (IContactDatastore)servProv.GetService(typeof(IContactDatastore));
        claims.Add(new Claim("Organisation", "org.Id"));
      }

      context.Principal.AddIdentity(new ClaimsIdentity(claims));
    }

    private sealed class CachedUserInfoResponse
    {
      public UserInfoResponse UserInfoResponse { get; }
      public DateTime Created { get; } = DateTime.UtcNow;

      public CachedUserInfoResponse(UserInfoResponse userInfoResponse)
      {
        UserInfoResponse = userInfoResponse;
      }
    }
  }
}

