using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// Additional data information attached to an <see cref="Edge"/>
  /// </summary>
  [Schema.Table(nameof(EdgeItemAttribute))]
  public sealed class EdgeItemAttribute : ItemAttribute
  {
    [Write(false)]
    public Guid EdgeId
    {
      get => OwnerId;

      set => OwnerId = value;
    }
  }
}
