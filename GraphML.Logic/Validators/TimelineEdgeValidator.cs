using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class TimelineEdgeValidator : OwnedValidatorBase<TimelineEdge>, ITimelineEdgeValidator
  {
    public TimelineEdgeValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
      RuleSet(nameof(ITimelineEdgeLogic.ByGraphItems), () =>
      {
      });
    }
  }
}
