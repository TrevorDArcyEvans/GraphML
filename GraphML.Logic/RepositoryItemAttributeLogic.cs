using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class RepositoryItemAttributeLogic : LogicBase<RepositoryItemAttribute>, IRepositoryItemAttributeLogic
  {
    public RepositoryItemAttributeLogic(
      IHttpContextAccessor context,
      IRepositoryItemAttributeDatastore datastore,
      IRepositoryItemAttributeValidator validator,
      IRepositoryItemAttributeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
