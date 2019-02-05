using Dapper.Contrib.Extensions;

namespace GraphML
{
  [Table(nameof(RepositoryManager))]
  public sealed class RepositoryManager : OwnedItem
  {
    public RepositoryManager() :
      base()
    {
    }

    public RepositoryManager(string orgId, string name) :
      base(orgId, name)
    {
    }
  }
}
