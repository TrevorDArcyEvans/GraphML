using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(ItemAttribute))]
  public sealed class EdgeItemAttribute : ItemAttribute
  {
  }
}
