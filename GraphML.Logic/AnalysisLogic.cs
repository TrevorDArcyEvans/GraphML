using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GraphML.Logic
{
  public sealed class AnalysisLogic : IAnalysisLogic
  {
    private readonly IHttpContextAccessor _context;
    private readonly IRequestMessageSender _sender;
    private readonly IGraphRequestValidator _graphRequestValidator;
    private readonly IGraphNodesRequestValidator _graphNodesRequestValidator;

    public AnalysisLogic(
      IHttpContextAccessor context,
      IRequestMessageSender sender,
      IGraphRequestValidator graphRequestValidator,
      IGraphNodesRequestValidator graphNodesRequestValidator)
    {
      _context = context;
      _sender = sender;
      _graphRequestValidator = graphRequestValidator;
      _graphNodesRequestValidator = graphNodesRequestValidator;
    }

    public void Degree(IDegreeRequest req)
    {
      // TODO   validation
      // requester is same org as graph
      Send(req);
    }

    public void Closeness(IClosenessRequest req)
    {
      // TODO   validation
      // requester is same org as graph
      Send(req);
    }

    public void Betweenness(IBetweennessRequest req)
    {
      // TODO   validation
      // requester is same org as graph
      Send(req);
    }

    public void FindShortestPaths(IFindShortestPathsRequest req)
    {
      // TODO   validation
      // requester is same org as graph
      // root and goal are in same graph (OwnerId)
      Send(req);
    }

    private void Send(IRequest req)
    {
      var jsonReq = JsonConvert.SerializeObject(req);
      _sender.Send(jsonReq);
    }
  }
}
