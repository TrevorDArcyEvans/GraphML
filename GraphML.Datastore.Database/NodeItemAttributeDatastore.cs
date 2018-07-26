using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class NodeItemAttributeDatastore : DatastoreBase<NodeItemAttribute>, INodeItemAttributeDatastore
  {
    public NodeItemAttributeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<NodeItemAttributeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
