using GraphML.Interfaces;
using System.Collections.Generic;

namespace GraphML.API.Server
{
  public interface IResultServer : IServerBase
  {
    void Delete(string correlationId);
    IEnumerable<IRequest> List(string contactId);
    IResult Retrieve(string correlationId);
  }
}
