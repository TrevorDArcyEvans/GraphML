using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;

namespace ChartJs.Blazor.GanttChart
{
    public class GanttConfig : ConfigBase<GanttOptions>
    {
        public GanttConfig() :
            base(ChartType.Gantt)
        {
        }
    }
}
