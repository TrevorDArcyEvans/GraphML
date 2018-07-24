using System;

namespace GraphML.Utils
{
  public static class ObjectExtensions
  {
    public static T ThrowIfNull<T>(this T obj, string paramName) where T : class
    {
      if (obj == null)
      {
        throw new ArgumentNullException(paramName);
      }
      return obj;
    }

    public static string ThrowIfNullOrWhiteSpace(this string obj, string paramName)
    {
      if (obj == null || string.IsNullOrWhiteSpace(obj))
      {
        throw new ArgumentNullException(paramName);
      }
      return obj;
    }

    public static Guid ThrowIfEmpty(this Guid obj, string paramName)
    {
      if (obj == null || obj == Guid.Empty)
      {
        throw new ArgumentNullException(paramName);
      }
      return obj;
    }
  }
}
