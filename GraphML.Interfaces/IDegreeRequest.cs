using System;

namespace GraphML.Interfaces
{
  public interface IDegreeRequest : IRequest
  {
    Guid GraphId { get; set; }
  }
}
