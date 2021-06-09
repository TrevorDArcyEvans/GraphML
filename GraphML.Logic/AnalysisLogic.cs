using FluentValidation;
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
      // requester is same org as graph
      var valRes = _graphRequestValidator.Validate(req, options => options.IncludeRuleSets(nameof(IAnalysisLogic.Degree)));
      if (!valRes.IsValid)
      {
        return;
      }

      Send(req);
    }

    public void Closeness(IClosenessRequest req)
    {
      // requester is same org as graph
      var valRes = _graphRequestValidator.Validate(req, options => options.IncludeRuleSets(nameof(IAnalysisLogic.Closeness)));
      if (!valRes.IsValid)
      {
        return;
      }

      Send(req);
    }

    public void Betweenness(IBetweennessRequest req)
    {
      // requester is same org as graph
      var valRes = _graphRequestValidator.Validate(req, options => options.IncludeRuleSets(nameof(IAnalysisLogic.Betweenness)));
      if (!valRes.IsValid)
      {
        return;
      }

      Send(req);
    }

    public void FindShortestPaths(IFindShortestPathsRequest req)
    {
      // requester is same org as graph
      // root and goal are in same graph
      var valRes = _graphNodesRequestValidator.Validate(req, options => options.IncludeRuleSets(nameof(IAnalysisLogic.FindShortestPaths)));
      if (!valRes.IsValid)
      {
        return;
      }
      Send(req);
    }

    public void FindDuplicates(IFindDuplicatesRequest req)
    {
      // requester is same org as graph
      var valRes = _graphRequestValidator.Validate(req, options => options.IncludeRuleSets(nameof(IAnalysisLogic.FindDuplicates)));
      if (!valRes.IsValid)
      {
        return;
      }
      Send(req);
    }

    private void Send(IRequest req)
    {
      var jsonReq = JsonConvert.SerializeObject(req);
      _sender.Send(jsonReq);
    }
  }
}
