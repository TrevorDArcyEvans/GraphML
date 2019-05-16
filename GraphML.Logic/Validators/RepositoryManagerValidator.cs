using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class RepositoryManagerValidator : OwnedValidatorBase<RepositoryManager>, IRepositoryManagerValidator
  {
    public RepositoryManagerValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
