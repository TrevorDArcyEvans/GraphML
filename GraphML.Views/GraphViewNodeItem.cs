using System;
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

    public GraphViewNodeItem(Guid graphViewId, Guid nodeId) :
      base(graphViewId, nameof(GraphViewNodeItem))
    {
      GraphItemId = nodeId;
    }
  }
}
