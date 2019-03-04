using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class ClosenessJob : JobBase, IClosenessJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<ClosenessJob> _logger;
    private readonly IGraphDatastore _graphDatastore;
    private readonly IResultLogic _resultLogic;

    public ClosenessJob(
      IConfiguration config,
      ILogger<ClosenessJob> logger,
      IGraphDatastore graphDatastore,
      IResultLogic resultLogic)
    {
      _config = config;
      _logger = logger;
      _graphDatastore = graphDatastore;
      _resultLogic = resultLogic;
    }

    public override void Run(IRequest req)
    {
      var closeReq = (IClosenessRequest)req;
      _logger.LogInformation($"ClosenessJob.Run --> {closeReq.GraphId} @ {closeReq.CorrelationId}");
    }
  }
}
