using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(RepositoryItemAttribute))]
  public sealed class RepositoryItemAttribute : ItemAttribute
  {
    [Write(false)]
    public Guid RepositoryId
    {
      get => OwnerId;
      set => OwnerId = value;
    }

  }
}
