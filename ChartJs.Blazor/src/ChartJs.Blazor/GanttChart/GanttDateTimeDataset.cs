using System.Collections.Generic;

namespace ChartJs.Blazor.GanttChart
{
    public class GanttDateTimeDataset : GanttDataset<GanttDateTimeData>
    {
        public GanttDateTimeDataset(IEnumerable<GanttDateTimeData> data) :
            base(data)
        {
        }
    }
}
