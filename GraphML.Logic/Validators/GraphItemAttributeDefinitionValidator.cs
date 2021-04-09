using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphItemAttributeDefinitionValidator : OwnedValidatorBase<GraphItemAttributeDefinition>, IGraphItemAttributeDefinitionValidator
  {
    public GraphItemAttributeDefinitionValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
