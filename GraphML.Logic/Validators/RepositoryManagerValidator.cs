using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class RepositoryManagerValidator : ValidatorBase<RepositoryManager>, IRepositoryManagerValidator
  {
    public RepositoryManagerValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
