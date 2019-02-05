using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class RepositoryValidator : OwnedValidatorBase<Repository>, IRepositoryValidator
  {
    public RepositoryValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
