using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class BetweennessJob : JobBase, IBetweennessJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<BetweennessJob> _logger;
    private readonly IGraphDatastore _graphDatastore;
    private readonly IResultLogic _resultLogic;

    public BetweennessJob(
      IConfiguration config,
      ILogger<BetweennessJob> logger,
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
      var closeReq = (IBetweennessRequest)req;
      _logger.LogInformation($"BetweennessJob.Run --> {closeReq.GraphId} @ {closeReq.CorrelationId}");
    }
  }
}
