﻿using Microsoft.Extensions.Logging;

namespace GraphML.UI.Desktop
{
  public sealed class GraphServer : OwnedItemServerBase<Graph>, IGraphServer
  {
    public GraphServer(
      IRestClientFactory clientFactory,
      ILogger<GraphServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Graph";
  }
}