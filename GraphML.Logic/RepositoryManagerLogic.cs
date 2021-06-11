using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class RepositoryManagerLogic : OwnedLogicBase<RepositoryManager>, IRepositoryManagerLogic
  {
    private readonly IRepositoryManagerDatastore _repoMgrDatastore;
    public RepositoryManagerLogic(
      IHttpContextAccessor context,
      ILogger<RepositoryManagerLogic> logger,
      IRepositoryManagerDatastore datastore,
      IRepositoryManagerValidator validator,
      IRepositoryManagerFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
      _repoMgrDatastore = datastore;
    }
  }
}
