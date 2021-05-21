using System.Collections.Generic;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.GanttChart.Axes;
using Newtonsoft.Json;

namespace ChartJs.Blazor.GanttChart
{
    public class GanttScales
    {
        [JsonProperty("xAxes")]
        public IList<GanttTimeAxis> XAxes { get; set; }

        [JsonProperty("yAxes")]
        public IList<CartesianAxis> YAxes { get; set; }
    }
}
