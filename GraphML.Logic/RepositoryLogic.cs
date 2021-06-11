using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class RepositoryLogic : OwnedLogicBase<Repository>, IRepositoryLogic
  {
    public RepositoryLogic(
      IHttpContextAccessor context,
      ILogger<RepositoryLogic> logger,
      IRepositoryDatastore datastore,
      IRepositoryValidator validator,
      IRepositoryFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
