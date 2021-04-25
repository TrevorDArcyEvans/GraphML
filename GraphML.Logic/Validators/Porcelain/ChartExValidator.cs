using GraphML.Interfaces;
using GraphML.Logic.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators.Porcelain
{
  public sealed class ChartExValidator : ValidatorBase<ChartEx>, IChartExValidator
  {
    public ChartExValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
  public sealed class ChartNodeExValidator : ValidatorBase<ChartNodeEx>, IChartNodeExValidator
  {
    public ChartNodeExValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
  public sealed class ChartEdgeExValidator : ValidatorBase<ChartEdgeEx>, IChartEdgeExValidator
  {
    public ChartEdgeExValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
