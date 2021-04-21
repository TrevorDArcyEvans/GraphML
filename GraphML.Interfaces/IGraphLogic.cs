﻿using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IGraphLogic : IOwnedLogic<Graph>
  {
    PagedDataEx<Graph> ByNodeId(Guid id, int pageIndex, int pageSize);

    PagedDataEx<Graph> ByEdgeId(Guid id, int pageIndex, int pageSize);
  }
}
