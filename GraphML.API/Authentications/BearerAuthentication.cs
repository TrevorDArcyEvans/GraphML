using GraphML.Interfaces.Authentications;
using GraphML.Utils;
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
#pragma warning disable CS1591
  public sealed class BearerAuthentication : IBearerAuthentication
  {
    private readonly IConfiguration _config;

    private static TimeSpan Expiry = TimeSpan.FromMinutes(60);

    // [bearerToken]-->[UserInfoResponse]
    private static Dictionary<string, CachedUserInfoResponse> _cache = new Dictionary<string, CachedUserInfoResponse>();

    public BearerAuthentication(
      IConfiguration config)
    {
      _config = config;
    }

    public async Task Authenticate(TokenValidatedContext context)
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
        var userInfoClient = new UserInfoClient(Settings.OIDC_USERINFO_URL(_config));
        response = await userInfoClient.GetAsync(bearerToken.Substring(7));
        _cache.Add(bearerToken, new CachedUserInfoResponse(response));
      }

      var userClaims = response.Claims;
      var claims = new List<Claim>(userClaims);

      // ClaimTypes.Email --> 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
      // but some OIDC providers use 'email'
      var email = userClaims.SingleOrDefault(x =>
        x.Type == ClaimTypes.Email ||
        x.Type.ToLowerInvariant() == "email")?.Value;
      if (!string.IsNullOrEmpty(email))
      {
        claims.Add(new Claim(ClaimTypes.Email, email));
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
#pragma warning restore CS1591
}

