using GraphML.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public abstract class OwnedValidatorBase<T> : ValidatorBase<T> where T : OwnedItem
  {
    public OwnedValidatorBase(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
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
