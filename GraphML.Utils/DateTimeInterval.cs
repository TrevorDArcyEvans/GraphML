using System;

namespace GraphML.Utils
{
  // Modelled after:
  //    https://github.com/nodatime/nodatime/blob/master/src/NodaTime/DateInterval.cs
  public sealed class DateTimeInterval
  {
    public DateTime Start { get; }
    public DateTime End { get; }

    public DateTimeInterval(DateTime start, DateTime end)
    {
      if (start.Kind != end.Kind)
      {
        throw new ArgumentException("Different kind of DateTime");
      }

      if (start > end)
      {
        throw new ArgumentOutOfRangeException("Start is after End");
      }

      Start = start;
      End = end;
    }

    public bool Contains(DateTime date)
    {
      if (date.Kind != Start.Kind)
      {
        throw new ArgumentException("Different kind of DateTime");
      }

      return Start <= date && date <= End;
    }

    public override string ToString()
    {
      return $"[{Start:o}, {End:o}]";
    }
  }
}
