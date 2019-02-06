﻿using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace GraphML.Logic
{
  public static class HttpContextAccessorExtensions
  {
    public static string Email(this IHttpContextAccessor context)
    {
      return context.HttpContext.Email();
    }

    public static string Email(this HttpContext context)
    {
      // ClaimTypes.Email --> 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
      // but some OIDC providers use 'email'
      return context.User.Claims
        .FirstOrDefault(x =>
          x.Type == ClaimTypes.Email ||
          x.Type.ToLowerInvariant() == "email")?.Value;
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
