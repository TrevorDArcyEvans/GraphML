using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class EdgeValidator : OwnedValidatorBase<Edge>, IEdgeValidator
  {
    public EdgeValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
      RuleSet(nameof(IEdgeLogic.ByNodeIds), () =>
      {
      });
    }
  }
}
