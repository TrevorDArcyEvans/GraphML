﻿using Flurl;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GraphML.UI.Desktop
{
  public sealed class RepositoryManagerServer : ServerBase<RepositoryManager>, IRepositoryManagerServer
  {
    public RepositoryManagerServer(
      IRestClientFactory clientFactory,
      ILogger<RepositoryManagerServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/RepositoryManager";

    public IEnumerable<RepositoryManager> GetAll()
    {
      var request = GetAllRequest(Url.Combine(ResourceBase, "GetAll"));
      var retval = GetResponse<IEnumerable<RepositoryManager>>(request);

      return retval;
    }
  }
}