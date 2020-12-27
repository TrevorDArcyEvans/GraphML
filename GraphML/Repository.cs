using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Repository))]
  public sealed class Repository : OwnedItem
  {
    [Write(false)]
    public Guid RepositoryManagerId
    {
      get => OwnerId;
      set => OwnerId = value;
    }

    public Repository() :
    base()
    {
    }

    public Repository(Guid repoMgr, Guid orgId, string name) :
      base(repoMgr, orgId, name)
    {
    }
  }
}
