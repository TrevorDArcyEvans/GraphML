using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IRepositoryItemLogic<T> : IOwnedLogic<T>
  {
    IEnumerable<T> GetParents(Guid itemId, int pageIndex, int pageSize);
  }
}
