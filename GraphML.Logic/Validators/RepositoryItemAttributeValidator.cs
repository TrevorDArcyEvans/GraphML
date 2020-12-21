using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class RepositoryItemAttributeValidator : OwnedValidatorBase<RepositoryItemAttribute>, IRepositoryItemAttributeValidator
  {
    public RepositoryItemAttributeValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
