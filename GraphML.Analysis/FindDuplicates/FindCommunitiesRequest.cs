using GraphML.Interfaces;

namespace GraphML.Analysis.FindDuplicates
{
  public sealed class FindCommunitiesRequest : RequestBase, IFindCommunitiesRequest
  {
    public override string JobType => typeof(IFindCommunitiesJob).AssemblyQualifiedName;
  }
}
