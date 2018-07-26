using Dapper.Contrib.Extensions;

namespace GraphML
{
  [Table(nameof(RepositoryItemAttribute))]
  public sealed class RepositoryItemAttribute : ItemAttribute
  {
  }
}
