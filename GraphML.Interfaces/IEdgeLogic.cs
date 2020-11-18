﻿using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IEdgeLogic : IOwnedLogic<Edge>
  {
    IEnumerable<Edge> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize);
  }
}
