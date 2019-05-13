﻿using Microsoft.Extensions.Logging;

namespace GraphML.UI.Desktop
{
  public sealed class RepositoryServer : OwnedItemServerBase<Repository>, IRepositoryServer
  {
    public RepositoryServer(
      IRestClientFactory clientFactory,
      ILogger<RepositoryServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Repository";
  }
}