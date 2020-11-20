using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
    public sealed class GraphNodeFilter : FilterBase<GraphNode>, IGraphNodeFilter
    {
        public GraphNodeFilter(IHttpContextAccessor context) :
            base(context)
        {
        }
    }
}