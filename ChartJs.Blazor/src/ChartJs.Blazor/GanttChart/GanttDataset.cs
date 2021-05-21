using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using ChartJs.Blazor.BarChart;

namespace ChartJs.Blazor.GanttChart
{
    public class GanttDataset<T> : Dataset<T>
    {
        public GanttDataset() :
            base(ChartType.Gantt)
        {
        }

        public GanttDataset(IEnumerable<T> data) :
            this()
        {
            AddRange(data);
        }

        protected GanttDataset(ChartType type) :
            base(type)
        {
        }

        public IndexableOption<string> BackgroundColor { get; set; }
        public IndexableOption<string> BorderColor { get; set; }
        public IndexableOption<BorderSkipped> BorderSkipped { get; set; }
        public IndexableOption<int> BorderWidth { get; set; }
        public BarThickness BarThickness { get; set; }
        public Clipping? Clip { get; set; }
        public IndexableOption<string> HoverBackgroundColor { get; set; }
        public IndexableOption<string> HoverBorderColor { get; set; }
        public IndexableOption<int> HoverBorderWidth { get; set; }
        public string Label { get; set; }
        public double? MaxBarThickness { get; set; }
        public double? MinBarLength { get; set; }
        public string Stack { get; set; }
        public int? Order { get; set; }

        [JsonProperty("xAxisID")]
        public string XAxisId { get; set; }

        [JsonProperty("yAxisID")]
        public string YAxisId { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; } = 0.125f;

        /// <summary>
        /// format for time scale:
        ///     string "xd yh zm ps qms",
        ///     object {d: x, h: y, m: z, s: p, ms:1} or
        ///     number - milliseconds
        /// where:
        ///     d: number of days,
        ///     h: number of hours,
        ///     m: number of minutes,
        ///     s: number of seconds,
        ///     ms: number of milliseconds.
        /// if the field is missing, then then is considered to be zero
        /// </summary>
        [JsonProperty("width")]
        public object Width { get; set; }
    }
}
