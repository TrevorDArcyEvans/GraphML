using System;

namespace GraphML.Interfaces
{
  public interface IBetweennessRequest : IRequest
  {
    Guid GraphId { get; set; }
  }
}
