using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class RepositoryItemAttributeValidator : OwnedValidatorBase<RepositoryItemAttribute>, IRepositoryItemAttributeValidator
  {
    public RepositoryItemAttributeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
