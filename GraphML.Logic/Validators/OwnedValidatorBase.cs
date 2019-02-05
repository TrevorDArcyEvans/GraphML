using GraphML.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public abstract class OwnedValidatorBase<T> : ValidatorBase<T>
  {
    public OwnedValidatorBase(IHttpContextAccessor context) : base(context)
    {
      RuleSet(nameof(IOwnedLogic<T>.ByOwners), () =>
      {
        RuleForByOwner();
      });
    }

    protected virtual void RuleForByOwner()
    {
    }
  }
}
