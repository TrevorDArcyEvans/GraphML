using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityServerAspNetIdentity
{
  public static class Settings
  {
    public static string KESTREL_URL(this IConfiguration config) =>
      Environment.GetEnvironmentVariable("KESTREL_URL") ??
      config["Kestrel:EndPoints:Http:Url"] ??
      "http://localhost:5000";

    public static IEnumerable<IdentityResource> IDENTITY_SERVER_IDS(this IConfiguration config)
    {
      var ids = config
          .GetSection("Identity_Server:Ids")
          .GetChildren()
          .Select(child => new IdentityResource(
              child["Name"],
              child.GetSection("ClaimTypes").GetChildren().Select(ct => ct.Value)));
      return ids;
    }

    public static IEnumerable<ApiResource> IDENTITY_SERVER_APIS(this IConfiguration config)
    {
      var apis = config
          .GetSection("Identity_Server:Apis")
          .GetChildren()
          .Select(child => new ApiResource(
              child["Name"],
              child["DisplayName"],
              child.GetSection("ClaimTypes").GetChildren().Select(ct => ct.Value)));
      return apis;
    }

    public static IEnumerable<Client> IDENTITY_SERVER_CLIENTS(this IConfiguration config)
    {
      var clients = config
          .GetSection("Identity_Server:Clients")
          .GetChildren()
          .Select(child => new Client
          {
            ClientId = child["ClientId"],
            ClientName = child["ClientName"],
            ClientSecrets = child
                  .GetSection("ClientSecrets")
                  .GetChildren()
                  .Select(cs => new Secret(cs.Value.Sha256()))
                  .ToList(),

            AllowedGrantTypes = new [] { GrantType.AuthorizationCode, GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },
            RequireConsent = false,
            RequirePkce = true,

                  // where to redirect to after login
                  RedirectUris = child
                  .GetSection("RedirectUris")
                  .GetChildren()
                  .Select(cs => cs.Value)
                  .ToList(),

            AllowedCorsOrigins = child
                  .GetSection("AllowedCorsOrigins")
                  .GetChildren()
                  .Select(cs => cs.Value)
                  .ToList(),

                  // where to redirect to after logout
                  PostLogoutRedirectUris = child
                  .GetSection("PostLogoutRedirectUris")
                  .GetChildren()
                  .Select(cs => cs.Value)
                  .ToList(),

                  // allowed scopes - include Api Resources and Identity Resources that may be accessed by this client
                  AllowedScopes = child
                  .GetSection("AllowedScopes")
                  .GetChildren()
                  .Select(cs => cs.Value)
                  .ToList(),

                  // include the refresh token
                  AllowOfflineAccess = true,

            ClientClaimsPrefix = "",
            AlwaysSendClientClaims = true,
            AlwaysIncludeUserClaimsInIdToken = true
          });
      return clients;
    }


    public static void DumpSettings(IConfiguration config)
    {
      var root = (IConfigurationRoot)config;
      var debugView = root.GetDebugView();
      Console.WriteLine(debugView);
      Console.WriteLine();

      Console.WriteLine("Settings:");
      Console.WriteLine($"  Identity_Server:");

      Console.WriteLine($"    Ids:");
      foreach (var id in config.IDENTITY_SERVER_IDS())
      {
        Console.WriteLine($"      {id.Name}");
        foreach (var uc in id.UserClaims)
        {
          Console.WriteLine($"        {uc}");
        }
      }

      Console.WriteLine($"    Apis:");
      foreach (var api in config.IDENTITY_SERVER_APIS())
      {
        Console.WriteLine($"      {api.Name} --> {api.DisplayName}");
        foreach (var uc in api.UserClaims)
        {
          Console.WriteLine($"        {uc}");
        }
      }

      Console.WriteLine($"    Clients:");
      foreach (var client in config.IDENTITY_SERVER_CLIENTS())
      {
        Console.WriteLine($"      {client.ClientId} --> {client.ClientName}");
        Console.WriteLine($"        ClientSecrets:");
        foreach (var cs in client.ClientSecrets)
        {
          Console.WriteLine($"          {cs.Value}");
        }
        Console.WriteLine($"        RedirectUris:");
        foreach (var cs in client.RedirectUris)
        {
          Console.WriteLine($"          {cs}");
        }
        Console.WriteLine($"        AllowedCorsOrigins:");
        foreach (var cs in client.AllowedCorsOrigins)
        {
          Console.WriteLine($"          {cs}");
        }
        Console.WriteLine($"        PostLogoutRedirectUris:");
        foreach (var cs in client.PostLogoutRedirectUris)
        {
          Console.WriteLine($"          {cs}");
        }
        Console.WriteLine($"        AllowedScopes:");
        foreach (var cs in client.AllowedScopes)
        {
          Console.WriteLine($"          {cs}");
        }
      }

      Console.WriteLine($"  KESTREL:");
      Console.WriteLine($"    KESTREL_URL                   : {config.KESTREL_URL()}");

      Console.WriteLine($"  CONNECTION_STRINGS:");
      Console.WriteLine($"    DEFAULT_CONNECTION            : {config.GetConnectionString("DefaultConnection")}");
    }
  }
}
