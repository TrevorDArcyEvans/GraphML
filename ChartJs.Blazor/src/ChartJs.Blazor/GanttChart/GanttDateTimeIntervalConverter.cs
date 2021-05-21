using System.Linq;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Time;
using Newtonsoft.Json;

namespace ChartJs.Blazor.GanttChart
{
    internal class GanttDateTimeIntervalConverter : JsonWriteOnlyConverter<GanttDateTimeInterval>
    {
        private readonly string FromName = typeof(GanttDateTimeInterval)
            .GetProperty(nameof(GanttDateTimeInterval.From))
            .GetCustomAttributes(false)
            .OfType<JsonPropertyAttribute>()
            .Single()
            .PropertyName;

        private readonly string ToName = typeof(GanttDateTimeInterval)
            .GetProperty(nameof(GanttDateTimeInterval.To))
            .GetCustomAttributes(false)
            .OfType<JsonPropertyAttribute>()
            .Single()
            .PropertyName;

        public override void WriteJson(JsonWriter writer, GanttDateTimeInterval value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(FromName);
            writer.WriteValue(value.From.ToJavascript());
            writer.WritePropertyName(ToName);
            writer.WriteValue(value.To.ToJavascript());
            writer.WriteEndObject();
        }
    }
}
