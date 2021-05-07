using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IGraphItemLogic<T> : IOwnedLogic<T> where T : GraphItem
  {
    IEnumerable<T> ByRepositoryItems(Guid graphId, IEnumerable<Guid> repoItemIds);
  }
}
