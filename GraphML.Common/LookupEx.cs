using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GraphML.Common
{
  public class LookupEx<TKey, TElement> : ILookup<TKey, TElement>
  {
    private readonly IEqualityComparer<TKey> _comparer = EqualityComparer<TKey>.Default;
    private Grouping<TKey, TElement>[] _groupings = new Grouping<TKey, TElement>[1];
    private Grouping<TKey, TElement>? _lastGrouping;
    private int _count;

    public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
    {
      Grouping<TKey, TElement>? g = _lastGrouping;
      if (g != null)
      {
        do
        {
          g = g._next;

          Debug.Assert(g != null);
          yield return g;
        } while (g != _lastGrouping);
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public bool Contains(TKey key) => GetGrouping(key, create: false) != null;

    public int Count => _count;

    public IEnumerable<TElement> this[TKey key]
    {
      get
      {
        Grouping<TKey, TElement>? grouping = GetGrouping(key, create: false);
        return grouping ?? Enumerable.Empty<TElement>();
      }
    }

    public Grouping<TKey, TElement>? GetGrouping(TKey key, bool create)
    {
      var hashCode = InternalGetHashCode(key);
      for (Grouping<TKey, TElement>? g = _groupings[hashCode % _groupings.Length]; g != null; g = g._hashNext)
      {
        if (g._hashCode == hashCode && _comparer.Equals(g._key, key))
        {
          return g;
        }
      }

      if (create)
      {
        if (_count == _groupings.Length)
        {
          Resize();
        }

        var index = hashCode % _groupings.Length;
        var g = new Grouping<TKey, TElement>(key, hashCode);
        g._hashNext = _groupings[index];
        _groupings[index] = g;
        if (_lastGrouping == null)
        {
          g._next = g;
        }
        else
        {
          g._next = _lastGrouping._next;
          _lastGrouping._next = g;
        }

        _lastGrouping = g;
        _count++;
        return g;
      }

      return null;
    }

    private int InternalGetHashCode(TKey key)
    {
      // Handle comparer implementations that throw when passed null
      return (key == null) ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;
    }

    private void Resize()
    {
      var newSize = checked((_count * 2) + 1);
      var newGroupings = new Grouping<TKey, TElement>[newSize];
      var g = _lastGrouping!;
      do
      {
        g = g._next!;
        int index = g._hashCode % newSize;
        g._hashNext = newGroupings[index];
        newGroupings[index] = g;
      } while (g != _lastGrouping);

      _groupings = newGroupings;
    }
  }
}
