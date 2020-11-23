﻿using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class RepositoryItemAttributeServer : OwnedItemServerBase<RepositoryItemAttribute>, IRepositoryItemAttributeServer
  {
    public RepositoryItemAttributeServer(
      IRestClientFactory clientFactory,
      ILogger<RepositoryItemAttributeServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(RepositoryItemAttribute)}";
  }
}
