using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// Additional data information attached to a <see cref="Graph"/>
  /// </summary>
  [Schema.Table(nameof(GraphItemAttribute))]
  public sealed class GraphItemAttribute : ItemAttribute
  {
    [Write(false)]
    public Guid GraphId
    {
      get => OwnerId;

      set => OwnerId = value;
    }
  }
}
