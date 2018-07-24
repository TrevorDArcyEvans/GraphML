using Dapper.Contrib.Extensions;

namespace GraphML
{
  [Table(nameof(Repository))]
  public sealed class Repository : AttributedItem
  {
    public Repository() :
    base()
    {
    }

    public Repository(string ownerId, string name) :
      base(ownerId, name)
    {
    }
  }
}
