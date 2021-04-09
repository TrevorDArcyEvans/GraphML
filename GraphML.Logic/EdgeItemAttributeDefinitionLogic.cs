using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class EdgeItemAttributeDefinitionLogic : OwnedLogicBase<EdgeItemAttributeDefinition>, IEdgeItemAttributeDefinitionLogic
  {
    public EdgeItemAttributeDefinitionLogic(
      IHttpContextAccessor context,
      IEdgeItemAttributeDefinitionDatastore datastore,
      IEdgeItemAttributeDefinitionValidator validator,
      IEdgeItemAttributeDefinitionFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
