using System;

namespace GraphML
{
  /// <summary>
  /// <para>A <see cref="TimelineNode"/> which appears in a <see cref="Timeline"/>.</para>
  /// <remarks>Name may be different to that of underlying Node</remarks>
  /// </summary>
  [System.ComponentModel.DataAnnotations.Schema.Table(nameof(TimelineNode))]
  public sealed class TimelineNode : TimelineItem
  {
    public TimelineNode() :
      base()
    {
    }

    public TimelineNode(Guid chart, Guid orgId, Guid graphItemId, string name) :
      base(chart, orgId, graphItemId, name)
    {
    }
  }
}
