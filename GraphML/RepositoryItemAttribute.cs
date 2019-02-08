using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(RepositoryItemAttribute))]
  public sealed class RepositoryItemAttribute : ItemAttribute
  {
  }
}
