using GraphML.Common;

namespace GraphML.Analysis.FindDuplicates
{
  public sealed class FindDuplicatesResult : ResultBase
  {
    // [ metaphone-key ] --> [ duplicate-GraphNode.Id[] ]
    public LookupEx<string, string[]> Result { get;  }

    public FindDuplicatesResult(LookupEx<string, string[]> result)
    {
      Result = result;
    }
  }
}
