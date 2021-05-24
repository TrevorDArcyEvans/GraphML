using System;

namespace GraphML.Utils
{
  /// <summary>
  /// An interval between two dates.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Equality is defined in a component-wise fashion: two date intervals are considered equal if their start dates are
  /// equal to each other and their end dates are equal to each other. Ordering between date intervals is not defined.
  /// </para>
  /// <para>
  /// The two dates must be in the same calendar, and the end date must not be earlier than the start date.
  /// </para>
  /// <para>
  /// The end date is deemed to be part of the range, as this matches many real life uses of
  /// date ranges. For example, if someone says "I'm going to be on holiday from Monday to Friday," they
  /// usually mean that Friday is part of their holiday.
  /// </para>
  /// <para>
  /// stolen from:<br/>
  ///     https://github.com/nodatime/nodatime/blob/master/src/NodaTime/DateInterval.cs
  /// </para>
  /// </remarks>
  public sealed class DateTimeInterval
  {
    /// <summary>
    /// Gets the start date of the interval.
    /// </summary>
    /// <value>The start date of the interval.</value>
    public DateTime Start { get; }

    /// <summary>
    /// Gets the end date of the interval.
    /// </summary>
    /// <value>The end date of the interval.</value>
    public DateTime End { get; }
    
    /// <summary>
    /// Constructs a date interval from a start date and an end date, both of which are included
    /// in the interval.
    /// </summary>
    /// <param name="start">Start date of the interval</param>
    /// <param name="end">End date of the interval</param>
    /// <exception cref="ArgumentException"><paramref name="end"/> is earlier than <paramref name="start"/>
    /// or the two dates are in different calendars.
    /// </exception>
    /// <returns>A date interval between the specified dates.</returns>
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

    /// <summary>
    /// Checks whether the given date is within this date interval. This requires
    /// that the date is not earlier than the start date, and not later than the end
    /// date.
    /// </summary>
    /// <param name="date">The date to check for containment within this interval.</param>
    /// <exception cref="ArgumentException"><paramref name="date"/> is not in the same
    /// calendar as the start and end date of this interval.</exception>
    /// <returns><c>true</c> if <paramref name="date"/> is within this interval; <c>false</c> otherwise.</returns>
    public bool Contains(DateTime date)
    {
      if (date.Kind != Start.Kind)
      {
        throw new ArgumentException("Different kind of DateTime");
      }

      return Start <= date && date <= End;
    }

    /// <summary>
    /// Returns a string representation of this interval.
    /// </summary>
    /// <returns>
    /// A string representation of this interval, as <c>[start, end]</c>,
    /// where "start" and "end" are the dates formatted using an ISO-8601 compatible pattern.
    /// </returns>
    public override string ToString()
    {
      return $"[{Start:o}, {End:o}]";
    }
  }
}
