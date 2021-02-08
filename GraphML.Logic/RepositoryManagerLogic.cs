using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
    public sealed class RepositoryManagerLogic : OwnedLogicBase<RepositoryManager>, IRepositoryManagerLogic
  {
    private readonly IRepositoryManagerDatastore _repoMgrDatastore;
    public RepositoryManagerLogic(
      IHttpContextAccessor context,
      IRepositoryManagerDatastore datastore,
      IRepositoryManagerValidator validator,
      IRepositoryManagerFilter filter) :
      base(context, datastore, validator, filter)
    {
      _repoMgrDatastore = datastore;
    }
  }
}
