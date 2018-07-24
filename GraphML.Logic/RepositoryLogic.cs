using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class RepositoryLogic : LogicBase<Repository>, IRepositoryLogic
  {
    public RepositoryLogic(
      IHttpContextAccessor context,
      IRepositoryDatastore datastore,
      IRepositoryValidator validator,
      IRepositoryFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
