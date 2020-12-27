using System;
using Dapper.Contrib.Extensions;
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

    public RepositoryManager(Guid org, string name) :
      base(org, org,name)
    {
    }
  }
}
