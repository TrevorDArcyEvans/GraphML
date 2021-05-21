using Newtonsoft.Json;

namespace ChartJs.Blazor.GanttChart
{
    public class GanttInterval<T>
    {
        [JsonProperty("from")]
        public T From { get; set; }

        [JsonProperty("to")]
        public T To { get; set; }
    }
}
