using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class RepositoryItemAttributeValidator : ValidatorBase<RepositoryItemAttribute>, IRepositoryItemAttributeValidator
  {
    public RepositoryItemAttributeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
