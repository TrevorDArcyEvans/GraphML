using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class NodeItemAttributeDefinitionLogic : OwnedLogicBase<NodeItemAttributeDefinition>, INodeItemAttributeDefinitionLogic
  {
    public NodeItemAttributeDefinitionLogic(
      IHttpContextAccessor context,
      ILogger<NodeItemAttributeDefinitionLogic> logger,
      INodeItemAttributeDefinitionDatastore datastore,
      INodeItemAttributeDefinitionValidator validator,
      INodeItemAttributeDefinitionFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
