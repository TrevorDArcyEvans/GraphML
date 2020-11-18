using System;

namespace GraphML.Interfaces
{
  public interface IClosenessRequest : IRequest
  {
    Guid GraphId { get; set; }
  }
}
