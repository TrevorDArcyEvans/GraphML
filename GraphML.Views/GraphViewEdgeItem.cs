using System;
using GraphML.Utils;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML.Views
{
  [Schema.Table(nameof(GraphViewEdgeItem))]
  public sealed class GraphViewEdgeItem : GraphViewItem
  {
    public GraphViewEdgeItem() :
      base()
    {
    }

    public GraphViewEdgeItem(Guid graphViewId, Guid edgeId) :
      base(graphViewId, nameof(GraphViewEdgeItem))
    {
      GraphItemId = edgeId;
    }
  }
}
