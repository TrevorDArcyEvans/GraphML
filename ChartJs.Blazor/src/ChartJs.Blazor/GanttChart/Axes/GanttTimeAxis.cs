using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using Newtonsoft.Json;

namespace ChartJs.Blazor.GanttChart.Axes
{
    public class GanttTimeAxis : TimeAxis
    {
        public override AxisType Type => AxisType.Gantt;

        [JsonProperty("isTime")]
        public bool IsTime => true;

        public double? BarPercentage { get; set; }
        public double? CategoryPercentage { get; set; }
        public BarThickness BarThickness { get; set; }
        public double? MaxBarThickness { get; set; }
        public double? MinBarLength { get; set; }
        public bool? OffsetGridLines
        {
            get => GridLines?.OffsetGridLines;
            set
            {
                if (GridLines == null)
                {
                    if (value == null)
                    {
                        return;
                    }
                    else
                    {
                        GridLines = new GridLines();
                    }
                }

                GridLines.OffsetGridLines = value;
            }
        }
        public bool? Stacked { get; set; }
    }
}

