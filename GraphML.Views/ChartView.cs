using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML.Views
{
  [Schema.Table(nameof(GraphView))]
  public sealed class ChartView : GraphView
  {
    public ChartView() :
      base()
    {
    }

    public ChartView(Guid graphId, string name) :
      base(graphId, name)
    {
      ViewType = typeof(ChartView).ToString();
    }
  }
}
