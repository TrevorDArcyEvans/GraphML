using GraphML.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GraphML.Logic
{
  public sealed class AnalysisLogic : IAnalysisLogic
  {
    private readonly IHttpContextAccessor _context;
    private readonly IRequestMessageSender _sender;

    // IGraphNodesRequestValidator --> all GraphNodes are in same graph
    //    IGraphNodeDatastore.ByIds --> GraphNode.GraphId[]

    // IGraphRequestValidator --> requester is same org as graph
    //    IGraphNodeDatastore.ByIds --> GraphNode.GraphId
    //    IGraphDatastore.ByIds --> Graph.RepositoryId
    //    IRepositoryDatastore.ByIds --> Repository.RepositoryManagerId
    //    IRepositoryManagerDatastore.ByIds --> RepositoruManager.OrganisationId
    //    IOrganisationDatastore.ByIds --> Organisation.Id
    //    IContactDatastore.ByEmail --> Contact.OrganisationId

    public AnalysisLogic(
      IHttpContextAccessor context,
      IRequestMessageSender sender)
    {
      _context = context;
      _sender = sender;
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
