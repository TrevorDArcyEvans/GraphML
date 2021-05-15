using System;
using Newtonsoft.Json;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// <para>A <see cref="GraphNode"/> which appears in a <see cref="Chart"/>.</para>
  /// <remarks>Name may be different to that of underlying Node</remarks>
  /// </summary>
  [Schema.Table(nameof(ChartNode))]
  public sealed class ChartNode : ChartItem
  {
    /// <summary>
    /// X coordinate in <see cref="Chart"/> space.
    /// </summary>
    [JsonProperty(nameof(X))]
    public int X { get; set; }

    /// <summary>
    /// Y coordinate in <see cref="Chart"/> space.
    /// </summary>
    [JsonProperty(nameof(Y))]
    public int Y { get; set; }
    
    /// <summary>
    /// Name of Font-Awesome <see cref="IconName"/> when rendering
    /// an Icon on a Diagram surface
    /// <remarks>
    /// If this is null, then no Icon is shown.
    /// </remarks>
    /// </summary>
    public string IconName { get; set; }

    public ChartNode() :
      base()
    {
    }

    public ChartNode(Guid chart, Guid orgId, Guid graphItemId, string name) :
      base(chart, orgId, graphItemId, name)
    {
    }
  }
}
