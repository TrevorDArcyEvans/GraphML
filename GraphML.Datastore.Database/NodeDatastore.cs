using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class NodeDatastore : OwnedItemDatastoreBase<Node>, INodeDatastore
  {
    public NodeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<NodeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
