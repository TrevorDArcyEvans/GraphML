using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GraphML
{
  /// <summary>
  /// <para>An <see cref="TimelineEdge"/> which appears in a <see cref="Timeline"/>.</para>
  /// <remarks>Name may be different to that of underlying Edge</remarks>
  /// </summary>
  [System.ComponentModel.DataAnnotations.Schema.Table(nameof(TimelineEdge))]
  public sealed class TimelineEdge : TimelineItem
  {
    [Required]
    [JsonProperty(nameof(TimelineSourceId))]
    public Guid TimelineSourceId { get; set; }

    [Required]
    [JsonProperty(nameof(TimelineTargetId))]
    public Guid TimelineTargetId { get; set; }

    public TimelineEdge() :
      base()
    {
    }

    public TimelineEdge(
      Guid chart,
      Guid orgId,
      Guid graphItemId,
      string name,
      Guid source,
      Guid target) :
      base(chart, orgId, graphItemId, name)
    {
      TimelineSourceId = source;
      TimelineTargetId = target;
    }
  }
}
