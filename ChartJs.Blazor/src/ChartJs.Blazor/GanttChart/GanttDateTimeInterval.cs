using System;
using Newtonsoft.Json;

namespace ChartJs.Blazor.GanttChart
{
    [JsonConverter(typeof(GanttDateTimeIntervalConverter))]
    public class GanttDateTimeInterval : GanttInterval<DateTime>
    {
    }
}
