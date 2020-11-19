using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(NodeItemAttribute))]
  public sealed class NodeItemAttribute : ItemAttribute
  {
    [Write(false)]
    public Guid NodeId
    {
      get => OwnerId;
      set => OwnerId = value;
    }

  }
}
