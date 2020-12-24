using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
    public interface IGraphNodesRequest : IRequest
    {
        IEnumerable<Guid> GraphNodes { get; set; }
    }
}