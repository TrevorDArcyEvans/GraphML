using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public abstract class GraphItemValidatorBase<T>: OwnedValidatorBase<T>, IGraphItemValidator<T> where T : GraphItem
  {
    public GraphItemValidatorBase(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
      RuleSet(nameof(IGraphItemLogic<T>.ByRepositoryItems), () =>
      {
      });
      RuleSet(nameof(IGraphItemLogic<T>.AddByFilter), () =>
      {
      });
    }
  }
}
