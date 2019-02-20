using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Utils
{
  /// <summary>
  /// A paged list of objects
  /// </summary>
  /// <typeparam name="T">type of objects in list</typeparam>
  public sealed class PaginatedList<T>
  {
    /// <summary>
    /// 1-based index of which page this page
    /// Defaults to 1
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Maximum number of items in this page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// List of items
    /// </summary>
    public IEnumerable<T> Items { get; set; }

    public PaginatedList()
    {
    }

    public PaginatedList(IEnumerable<T> items, int pageIndex, int pageSize)
    {
      PageIndex = pageIndex;
      PageSize = pageSize;
      Items = items;
    }
  }
}
