using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class RepositoryManagerLogic : LogicBase<RepositoryManager>, IRepositoryManagerLogic
  {
    public RepositoryManagerLogic(
      IHttpContextAccessor context,
      IRepositoryManagerDatastore datastore,
      IRepositoryManagerValidator validator,
      IRepositoryManagerFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
