using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class RepositoryValidator : ValidatorBase<Repository>, IRepositoryValidator
  {
    public RepositoryValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
