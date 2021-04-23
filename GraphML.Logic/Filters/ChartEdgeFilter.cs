using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class ChartEdgeFilter : FilterBase<ChartEdge>, IChartEdgeFilter
  {
    public ChartEdgeFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
