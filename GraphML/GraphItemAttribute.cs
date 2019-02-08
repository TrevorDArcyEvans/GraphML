using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(GraphItemAttribute))]
  public sealed class GraphItemAttribute : ItemAttribute
  {
  }
}
