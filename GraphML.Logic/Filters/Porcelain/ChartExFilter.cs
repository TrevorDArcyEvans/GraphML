using GraphML.Interfaces;
using GraphML.Logic.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters.Porcelain
{
  public sealed class ChartExFilter : FilterBase<ChartEx>, IChartExFilter
  {
    public ChartExFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
  public sealed class ChartNodeExFilter : FilterBase<ChartNodeEx>, IChartNodeExFilter
  {
    public ChartNodeExFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
  public sealed class ChartEdgeExFilter : FilterBase<ChartEdgeEx>, IChartEdgeExFilter
  {
    public ChartEdgeExFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
