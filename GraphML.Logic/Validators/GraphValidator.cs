using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphValidator : OwnedValidatorBase<Graph>, IGraphValidator
  {
    public GraphValidator(IHttpContextAccessor context) :
      base(context)
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
