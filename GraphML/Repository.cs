using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Repository))]
  public sealed class Repository : OwnedItem
  {
    public Repository() :
    base()
    {
    }

    public Repository(Guid repoMgrId, string name) :
      base(repoMgrId, name)
    {
    }
  }
}
