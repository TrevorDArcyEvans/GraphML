using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class GraphItemAttributeDefinitionLogic : OwnedLogicBase<GraphItemAttributeDefinition>, IGraphItemAttributeDefinitionLogic
  {
    public GraphItemAttributeDefinitionLogic(
      IHttpContextAccessor context,
      ILogger<GraphItemAttributeDefinitionLogic> logger,
      IGraphItemAttributeDefinitionDatastore datastore,
      IGraphItemAttributeDefinitionValidator validator,
      IGraphItemAttributeDefinitionFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
