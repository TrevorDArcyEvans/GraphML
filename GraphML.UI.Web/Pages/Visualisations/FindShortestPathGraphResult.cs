using System.Collections.Generic;

namespace GraphML.UI.Web.Pages.Visualisations
{
    public sealed class FindShortestPathGraphResult
    {
        public IEnumerable<GraphEdge> Path { get; set; }
        public double Cost { get; set; }
    }
}
