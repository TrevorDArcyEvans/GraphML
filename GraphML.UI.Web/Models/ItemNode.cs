using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace GraphML.UI.Web.Models
{
  public class ItemNode : NodeModel
  {
    /// <summary>
    /// A <see cref="ChartNode"/> displayed in a <see cref="Chart"/>
    /// </summary>
    /// <param name="chartNode">Underlying <see cref="ChartNode"/></param>
    /// <param name="pos">Location of <see cref="ItemNode"/> on <see cref="Chart"/></param>
    public ItemNode(ChartNode chartNode, Point pos) :
      base(chartNode.Id.ToString(), pos)
    {
      ChartNode = chartNode;
    }

    public ChartNode ChartNode { get; }
    
    public string Name => ChartNode.Name;
  }
}
