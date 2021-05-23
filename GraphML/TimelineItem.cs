using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace GraphML
{
  /// <summary>
  /// Something which is in a <see cref="Timeline"/>, either a <see cref="TimelineNode"/> or a <see cref="TimelineEdge"/>
  /// </summary>
  public abstract class TimelineItem : OwnedItem
  {
    [Write(false)]
    public Guid ChartId
    {
      get => OwnerId;

      set => OwnerId = value;
    }

    /// <summary>
    /// Unique identifier of underlying <see cref="GraphItem"/>, either a <see cref="GraphNode"/> or a <see cref="GraphEdge"/>
    /// </summary>
    [Required]
    [JsonProperty(nameof(GraphItemId))]
    public Guid GraphItemId { get; set; }

    public TimelineItem() :
      base()
    {
    }

    public TimelineItem(Guid chart, Guid orgId, Guid graphItemId, string name) :
      base(chart, orgId, name)
    {
      GraphItemId = graphItemId;
    }
  }
}
