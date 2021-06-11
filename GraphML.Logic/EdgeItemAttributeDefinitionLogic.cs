using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class EdgeItemAttributeDefinitionLogic : OwnedLogicBase<EdgeItemAttributeDefinition>, IEdgeItemAttributeDefinitionLogic
  {
    public EdgeItemAttributeDefinitionLogic(
      IHttpContextAccessor context,
      ILogger<EdgeItemAttributeDefinitionLogic> logger,
      IEdgeItemAttributeDefinitionDatastore datastore,
      IEdgeItemAttributeDefinitionValidator validator,
      IEdgeItemAttributeDefinitionFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
