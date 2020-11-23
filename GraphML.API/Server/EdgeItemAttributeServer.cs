﻿using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class EdgeItemAttributeServer : OwnedItemServerBase<EdgeItemAttribute>, IEdgeItemAttributeServer
  {
    public EdgeItemAttributeServer(
      IRestClientFactory clientFactory,
      ILogger<EdgeItemAttributeServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(EdgeItemAttribute)}";
  }
}
