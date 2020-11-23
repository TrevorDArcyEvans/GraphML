﻿using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
    public sealed class GraphEdgeFilter : FilterBase<GraphEdge>, IGraphEdgeFilter
    {
        public GraphEdgeFilter(IHttpContextAccessor context) :
            base(context)
        {
        }
    }
}