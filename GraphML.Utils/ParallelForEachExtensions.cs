using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphML.Utils
{
  public static class ParallelForEachExtensions
  {
    // https://medium.com/@alex.puiu/parallel-foreach-async-in-c-36756f8ebe62
    public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
    {
      async Task AwaitPartition(IEnumerator<T> partition)
      {
        using (partition)
        {
          while (partition.MoveNext())
          {
            await body(partition.Current);
          }
        }
      }

      return Task.WhenAll(
        Partitioner
          .Create(source)
          .GetPartitions(dop)
          .AsParallel()
          .Select(p => AwaitPartition(p)));
    }
  }
}
