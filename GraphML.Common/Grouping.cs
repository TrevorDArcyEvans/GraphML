using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Common
{
  public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>
  {
    internal readonly TKey _key;
    internal readonly int _hashCode;
    private TElement[] _elements;
    private int _count;
    internal Grouping<TKey, TElement>? _hashNext;
    internal Grouping<TKey, TElement>? _next;

    public Grouping(TKey key, int hashCode)
    {
      _key = key;
      _hashCode = hashCode;
      _elements = new TElement[1];
    }

    public void Add(TElement element)
    {
      if (_elements.Length == _count)
      {
        Array.Resize(ref _elements, checked(_count * 2));
      }

      _elements[_count] = element;
      _count++;
    }

    internal void Trim()
    {
      if (_elements.Length != _count)
      {
        Array.Resize(ref _elements, _count);
      }
    }

    public IEnumerator<TElement> GetEnumerator()
    {
      for (int i = 0; i < _count; i++)
      {
        yield return _elements[i];
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // DDB195907: implement IGrouping<>.Key implicitly
    // so that WPF binding works on this property.
    public TKey Key => _key;

    int ICollection<TElement>.Count => _count;

    bool ICollection<TElement>.IsReadOnly => true;

    void ICollection<TElement>.Add(TElement item) => throw new NotSupportedException();

    void ICollection<TElement>.Clear() => throw new NotSupportedException();

    bool ICollection<TElement>.Contains(TElement item) => Array.IndexOf(_elements, item, 0, _count) >= 0;

    void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex) =>
      Array.Copy(_elements, 0, array, arrayIndex, _count);

    bool ICollection<TElement>.Remove(TElement item)
    {
      throw new NotSupportedException();
      return false;
    }

    int IList<TElement>.IndexOf(TElement item) => Array.IndexOf(_elements, item, 0, _count);

    void IList<TElement>.Insert(int index, TElement item) => throw new NotSupportedException();

    void IList<TElement>.RemoveAt(int index) => throw new NotSupportedException();

    TElement IList<TElement>.this[int index]
    {
      get
      {
        if (index < 0 || index >= _count)
        {
          throw new ArgumentOutOfRangeException($"Index = {index}");
        }

        return _elements[index];
      }

      set
      {
        throw new NotSupportedException();
      }
    }
  }
}
