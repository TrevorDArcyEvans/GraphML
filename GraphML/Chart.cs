using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// A 2D pictorial representation of a subset of <see cref="Node"/>  and <see cref="Edge"/> from a <see cref="Graph"/>.
  /// Generally used to visualise analysis results.
  /// Default implementation is a Diagram.
  /// Layout algorithms can be applied to change the position of Nodes and Edges.
  /// </summary>
  [Schema.Table(nameof(Chart))]
  public sealed class Chart : OwnedItem
  {
    [Write(false)]
    public Guid GraphId
    {
      get => OwnerId;

      set => OwnerId = value;
    }

    public Chart() :
      base()
    {
    }

    public Chart(Guid graph, Guid orgId, string name) :
      base(graph, orgId, name)
    {
    }
  }
}
