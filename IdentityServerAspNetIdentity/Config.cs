// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerAspNetIdentity
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> Ids =>
      new List<IdentityResource>
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
        // Identity Resource for custom user claim type
        new IdentityResource("appuser_claim", new []{ "appuser_claim" })
      };


    public static IEnumerable<ApiResource> Apis =>
      new[]
      {   
          // Identity API, consumes user claim type 'appuser_claim'
          // By assigning the user claim to the api resource,
          // we are instructing Identity Server to include that claim in
          // Access tokens for this resource.
          new ApiResource("identityApi",
                "Identity Claims Api",
                 new []
                 {
                   "appuser_claim",
                   "email",
                   "email_verified",
                   "website"
                 })
      };

    public static IEnumerable<Client> Clients =>
      new[]
      {
                // interactive ASP.NET Core Blazor Server Client
                new Client
        {
          ClientId = "BlazorID_App", // TODO settings
          ClientName="Blazor Server App - Identity Claims", // TODO settings
          ClientSecrets = { new Secret("secret".Sha256()) }, // TODO settings

          AllowedGrantTypes = GrantTypes.Code,
          RequireConsent = false,
          RequirePkce = true,
                
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" }, // TODO settings

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" }, // TODO settings

                    // allowed scopes - include Api Resources and Identity Resources that may be accessed by this client
                    AllowedScopes = { "openid", "profile", "email", "identityApi", "appuser_claim" }, // TODO settings

                    // include the refresh token
                   AllowOfflineAccess = true,

           ClientClaimsPrefix = "",
           AlwaysSendClientClaims = true,
           AlwaysIncludeUserClaimsInIdToken = true
        },
                // ASP.NET Cor API Client
                new Client
        {
          ClientId = "graphml_api_swagger", // TODO settings
          ClientName="Swagger UI for GraphML API", // TODO settings
          ClientSecrets = { new Secret("secret".Sha256()) }, // TODO settings

          AllowedGrantTypes = GrantTypes.Code,
          RequireConsent = false,
          RequirePkce = true,
                
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5001/swagger/oauth2-redirect.html" }, // TODO settings

          AllowedCorsOrigins = {"https://localhost:5001"}, // TODO settings

                    // where to redirect to after logout
                    //PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" }, // TODO settings

                    // allowed scopes - include Api Resources and Identity Resources that may be accessed by this client
                    AllowedScopes = { "identityApi" }, // TODO settings

                    // include the refresh token
                   AllowOfflineAccess = true,

           ClientClaimsPrefix = "",
           AlwaysSendClientClaims = true,
           AlwaysIncludeUserClaimsInIdToken = true
        }
      };
  }
}
