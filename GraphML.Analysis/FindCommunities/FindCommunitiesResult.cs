using System;
using System.Collections.Generic;

namespace GraphML.Analysis.FindCommunities
{
  public sealed class FindCommunitiesResult : ResultBase
  {
    /// <summary>
    /// List of communities
    /// </summary>
    public List<List<Guid>> Result { get;  }

    public FindCommunitiesResult(List<List<Guid>> result)
    {
      Result = result;
    }
  }
}
