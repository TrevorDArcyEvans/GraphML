using System.Linq;
using Blazor.Diagrams.Core.Models;

namespace GraphML.UI.Web.Models
{
  public sealed class DiagramLink : LinkModel
  {
    /// <summary>
    /// A <see cref="ChartEdge"/> displayed in a <see cref="Chart"/>
    /// </summary>
    /// <param name="chartEdge">Underlying <see cref="ChartEdge"/></param>
    /// <param name="sourceNode"></param>
    /// <param name="targetNode"></param>
    public DiagramLink(ChartEdge chartEdge, NodeModel sourceNode, NodeModel? targetNode) :
      base(chartEdge.Id.ToString(), sourceNode, targetNode)
    {
      ChartEdge = chartEdge;
      Labels.Add(new DiagramLinkLabel(this, chartEdge.Name));
    }

    public ChartEdge ChartEdge { get; }

    public string Name
    {
      get => ChartEdge.Name;

      set => ChartEdge.Name = Labels.Single().Content = value;
    }
  }
}
