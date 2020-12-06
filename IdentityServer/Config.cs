using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> IdentityResources =>
      new IdentityResource[]
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
        new IdentityResource(JwtClaimTypes.Role, new [] { JwtClaimTypes.Role })
      };

    public static IEnumerable<ApiScope> ApiScopes =>
      new[]
      {
        new ApiScope("api1", "Full access to API #1") // "full access" scope
      };

    public static IEnumerable<ApiResource> ApiResources =>
      new[]
      {
        new ApiResource("api1", "API #1", new[] { JwtClaimTypes.Role, JwtClaimTypes.Email })
        {
          Scopes = { "api1" }
        }
      };

    public static IEnumerable<Client> Clients =>
      new[]
        {
          // GraphML API
          new Client
          {
            ClientId = "graphml_api_swagger",
            ClientName = "Swagger UI for GraphML API",
            ClientSecrets =
            {
              new Secret("password".Sha256()) // TODO  GraphML password
            },
            AlwaysIncludeUserClaimsInIdToken = true,
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,
            RedirectUris =
            {
              "https://localhost:5001/swagger/oauth2-redirect.html" // TODO   GraphML RedirectUris
            },
            AllowedCorsOrigins =
            {
              "https://localhost:5001" // TODO    GraphML AllowedCorsOrigins
            },
            AllowedScopes =
            {
              "api1"
            }
          }
        };
  }
}
