using System;
using Dapper.Contrib.Extensions;

namespace GraphML
{
  /// <summary>
  /// A 2D pictorial representation of a subset of <see cref="Node"/>  and <see cref="Edge"/> from a <see cref="Graph"/>.
  /// Generally used to visualise temporal (time based) data.
  /// Default implementation is a gantt chart.
  /// </summary>
  [System.ComponentModel.DataAnnotations.Schema.Table(nameof(Timeline))]
  public sealed class Timeline : OwnedItem
  {
    [Write(false)]
    public Guid GraphId
    {
      get => OwnerId;

      set => OwnerId = value;
    }

    public Timeline() :
      base()
    {
    }

    public Timeline(Guid graph, Guid orgId, string name) :
      base(graph, orgId, name)
    {
    }
  }
}
