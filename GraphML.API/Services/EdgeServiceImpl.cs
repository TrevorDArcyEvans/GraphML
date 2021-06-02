using System;
using System.Threading.Tasks;
using GraphML.Interfaces;
using GraphML.RPC;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Services
{
  public sealed class EdgeServiceImpl : EdgeService.EdgeServiceBase
  {
    private readonly ILogger<EdgeServiceImpl> _logger;
    private readonly IEdgeDatastore _datastore;

    public EdgeServiceImpl(
      ILogger<EdgeServiceImpl> logger, 
      IEdgeDatastore datastore)
    {
      _logger = logger;
      _datastore = datastore;
    }

    public override Task<EdgeByOwnerResponse> ByOwner(EdgeByOwnerRequest request, ServerCallContext context)
    {
      // TODO   authentication/authorisation
      var ownerId = Guid.Parse(request.OwnerId);
      var dataPage = _datastore.ByOwners(new[] { ownerId }, 0, int.MaxValue, null);
      var data = dataPage.Items;
      var response = new EdgeByOwnerResponse();
      foreach (var dataEdge in data)
      {
        var rpcEdge = new RPC.Edge
        {
          Base = new RPC.RepositoryItem
          {
            Base = new RPC.OwnedItem
            {
              Base = new RPC.Item
              {
                Name = dataEdge.Name,
                Id = dataEdge.Id.ToString(),
                OrganisationId = dataEdge.OrganisationId.ToString()
              },
              OwnerId = dataEdge.OwnerId.ToString()
            },NextId = dataEdge.NextId.ToString()
          },
          SourceId = dataEdge.SourceId.ToString(),
          TargetId = dataEdge.TargetId.ToString()
        };
        response.Edges.Add(rpcEdge);
      }
      
      return Task.FromResult(response);
    }
  }
}
