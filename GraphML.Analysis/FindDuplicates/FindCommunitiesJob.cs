using System;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraphML.Analysis.FindDuplicates
{
  public sealed class FindCommunitiesJob : IFindCommunitiesJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<FindCommunitiesJob> _logger;
    private readonly IGraphNodeDatastore _nodeDatastore;
    private readonly IResultLogic _resultLogic;

    public FindCommunitiesJob(
      IConfiguration config,
      ILogger<FindCommunitiesJob> logger,
      IGraphNodeDatastore nodeDatastore,
      IResultLogic resultLogic)
    {
      _config = config;
      _logger = logger;
      _nodeDatastore = nodeDatastore;
      _resultLogic = resultLogic;
    }

    public void Run(IRequest req)
    {
      throw new NotImplementedException();
    }
  }
}
