using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// Additional data information attached to a <see cref="Repository"/>
  /// </summary>
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
