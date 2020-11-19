using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(RepositoryManager))]
  public sealed class RepositoryManager : OwnedItem
  {
    [Write(false)]
    public Guid OrganisationId
    {
      get => OwnerId;
      set => OwnerId = value;
    }

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
