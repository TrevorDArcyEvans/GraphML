using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(NodeItemAttribute))]
  public sealed class NodeItemAttribute : ItemAttribute
  {
  }
}
