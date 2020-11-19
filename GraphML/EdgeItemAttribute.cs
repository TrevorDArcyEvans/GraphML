using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
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
