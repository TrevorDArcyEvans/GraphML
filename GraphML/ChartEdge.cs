using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// <para>An <see cref="GraphEdge"/> which appears in a <see cref="Chart"/>.</para>
  /// <remarks>Name may be different to that of underlying Edge</remarks>
  /// </summary>
  [Schema.Table(nameof(ChartEdge))]
  public sealed class ChartEdge : ChartItem
  {
    [Required]
    [JsonProperty(nameof(ChartSourceId))]
    public Guid ChartSourceId { get; set; }

    [Required]
    [JsonProperty(nameof(ChartTargetId))]
    public Guid ChartTargetId { get; set; }

    public ChartEdge() :
      base()
    {
    }

    public ChartEdge(
      Guid chart,
      Guid orgId,
      Guid graphItemId,
      string name,
      Guid source,
      Guid target) :
      base(chart, orgId, graphItemId, name)
    {
      ChartSourceId = source;
      ChartTargetId = target;
    }
  }
}
