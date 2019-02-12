using GraphML.Interfaces.Authentications;
using GraphML.Utils;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
      throw new NotImplementedException();
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

