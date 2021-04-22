using System;
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
    public ChartEdge() :
      base()
    {
    }

    public ChartEdge(Guid chart, Guid orgId, Guid graphItemId, string name) :
      base(chart, orgId, graphItemId, name)
    {
    }
  }
}
