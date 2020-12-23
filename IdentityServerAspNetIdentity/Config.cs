// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
      public static IEnumerable<IdentityResource> Ids(IConfiguration config)
      {
          var retval = new List<IdentityResource>(
              new IdentityResource[]
              {
                  new IdentityResources.OpenId(),
                  new IdentityResources.Profile(),
                  new IdentityResources.Email()
              });
          retval.AddRange(config.IDENTITY_SERVER_IDS());

          return retval;
      }

      public static IEnumerable<ApiResource> Apis(IConfiguration config) => config.IDENTITY_SERVER_APIS();

      public static IEnumerable<Client> Clients(IConfiguration config) => config.IDENTITY_SERVER_CLIENTS();
    }
}
