using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class GraphItemAttributeDefinitionDatastore : OwnedItemDatastore<GraphItemAttributeDefinition>, IGraphItemAttributeDefinitionDatastore
  {
    public GraphItemAttributeDefinitionDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphItemAttributeDefinitionDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
