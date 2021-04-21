﻿using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IOwnedLogic<T> : ILogic<T>
  {
    PagedDataEx<T> ByOwners(IEnumerable<Guid> ownerIds, int pageIndex, int pageSize);
    int Count(Guid ownerId);
  }
}
