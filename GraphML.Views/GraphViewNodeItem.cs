using GraphML.Utils;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML.Views
{
  [Schema.Table(nameof(GraphViewNodeItem))]
  public sealed class GraphViewNodeItem : GraphViewItem
  {
    public GraphViewNodeItem() :
      base()
    {
    }

    public GraphViewNodeItem(string graphViewId, string nodeId) :
      base(graphViewId, nameof(GraphViewNodeItem))
    {
      GraphItemId = nodeId.ThrowIfNullOrWhiteSpace(nameof(nodeId));
    }
  }
}
