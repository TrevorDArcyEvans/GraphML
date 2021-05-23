using System;
using Dapper.Contrib.Extensions;
using GraphML.Utils;

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
    /// <summary>
    /// Unique identifier of <see cref="EdgeItemAttributeDefinition"/> from which to extract <see cref="DateTimeInterval"/>
    /// </summary>
    public Guid DateTimeIntervalAttributeDefinitionId { get; set; }
    
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

    public Timeline(Guid graph, Guid orgId, string name, Guid intervalAttrDef) :
      base(graph, orgId, name)
    {
      DateTimeIntervalAttributeDefinitionId = intervalAttrDef;
    }
  }
}
