using System;

namespace GraphML.Interfaces
{
  public interface IGraphRequest : IRequest
  {
    Guid GraphId { get; set; }
  }
}
