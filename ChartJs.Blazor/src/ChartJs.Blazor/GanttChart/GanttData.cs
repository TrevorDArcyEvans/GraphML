using Newtonsoft.Json;

namespace ChartJs.Blazor.GanttChart
{
    public class GanttData<T>
    {
        [JsonProperty("x")]
        public GanttInterval<T> X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }
}
