using System.Collections.Generic;

namespace GraphML
{
  /// <summary>
  /// Data structure specifically intended for use with MatBlazor MatTable.
  /// MatTable will ask for a subset (aka page) of data from a potentially
  /// large set of data.  In order to implement paging (in the UI), MatTable
  /// needs to know the <see cref="TotalCount"/> in all the data.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public sealed class PagedDataEx<T>
  {
    /// <summary>
    /// Total number of items in data query,
    /// to assist in paging through all data
    /// <remarks>
    /// BEWARE:  this is greater than or equal to <see cref="Items"/>
    /// </remarks>
    /// </summary>
    public long TotalCount { get; set; }
    
    /// <summary>
    /// Entities in requested page of data
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();
  }
}
