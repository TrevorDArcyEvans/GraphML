using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML.Views
{
  [Schema.Table(nameof(GraphViewNodeItemAttribute))]
  public sealed class GraphViewNodeItemAttribute : GraphViewItemAttribute
  {
  }
}
