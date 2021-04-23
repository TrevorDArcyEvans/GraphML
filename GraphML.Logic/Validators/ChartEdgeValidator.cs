using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class ChartEdgeValidator : OwnedValidatorBase<ChartEdge>, IChartEdgeValidator
  {
    public ChartEdgeValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
