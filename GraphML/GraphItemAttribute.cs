using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
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
