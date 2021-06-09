using GraphML.Interfaces;

namespace GraphML.Analysis.FindDuplicates
{
  public sealed class FindDuplicatesRequest : RequestBase, IFindDuplicatesRequest
  {
    public override string JobType => typeof(IFindDuplicatesJob).AssemblyQualifiedName;
    public int MinMatchingKeyLength { get; set; }
  }
}
