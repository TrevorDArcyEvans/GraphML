using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphValidator : OwnedValidatorBase<Graph>, IGraphValidator
  {
    public GraphValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
      RuleSet(nameof(IGraphLogic.ByNodeId), () =>
      {
        RuleForByNodeId();
      });
      RuleSet(nameof(IGraphLogic.ByEdgeId), () =>
      {
        RuleForByEdgeId();
      });
    }

    public void RuleForByNodeId()
    {
    }

    public void RuleForByEdgeId()
    {
    }
  }
}
