using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class TimelineNodeValidator : OwnedValidatorBase<TimelineNode>, ITimelineNodeValidator
  {
    public TimelineNodeValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
      RuleSet(nameof(ITimelineNodeLogic.ByGraphItems), () =>
      {
      });
    }
  }
}
