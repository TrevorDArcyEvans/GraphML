using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML.Views
{
  [Schema.Table(nameof(GraphViewEdgeItemAttribute))]
  public sealed class GraphViewEdgeItemAttribute : GraphViewItemAttribute
  {
  }
}
