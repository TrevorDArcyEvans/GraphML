using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class RepositoryItemAttributeLogic : OwnedLogicBase<RepositoryItemAttribute>, IRepositoryItemAttributeLogic
  {
    public RepositoryItemAttributeLogic(
      IHttpContextAccessor context,
      ILogger<RepositoryItemAttributeLogic> logger,
      IRepositoryItemAttributeDatastore datastore,
      IRepositoryItemAttributeValidator validator,
      IRepositoryItemAttributeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
