using System.Collections.Generic;
using System.Linq;
using GraphML.Common;

namespace GraphML.Analysis.FindDuplicates
{
  public sealed class FindDuplicatesResult : ResultBase
  {
    /// <summary>
    /// map of:
    ///   [ metaphone-key ] --> [ duplicate-GraphNode.Id[] ]
    /// <remarks>
    /// Use <see cref="FindDuplicatesResultSerializer"/> to convert to/from JSON
    /// </remarks>
    /// </summary>
    public List<IGrouping<string, string[]>> Result { get;  }

    public FindDuplicatesResult(List<IGrouping<string, string[]>> result)
    {
      Result = result;
    }
  }
}
