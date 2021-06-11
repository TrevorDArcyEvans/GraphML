using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class RepositoryItemAttributeDefinitionLogic : OwnedLogicBase<RepositoryItemAttributeDefinition>, IRepositoryItemAttributeDefinitionLogic
  {
    public RepositoryItemAttributeDefinitionLogic(
      IHttpContextAccessor context,
      ILogger<RepositoryItemAttributeDefinitionLogic> logger,
      IRepositoryItemAttributeDefinitionDatastore datastore,
      IRepositoryItemAttributeDefinitionValidator validator,
      IRepositoryItemAttributeDefinitionFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
