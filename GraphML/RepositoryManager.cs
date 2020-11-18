using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(RepositoryManager))]
  public sealed class RepositoryManager : OwnedItem
  {
    public RepositoryManager() :
      base()
    {
    }

    public RepositoryManager(Guid orgId, string name) :
      base(orgId, name)
    {
    }
  }
}
