using GraphML.Interfaces;
using System.Collections.Generic;

namespace GraphML.UI.Desktop
{
  public interface IResultServer : IServerBase
  {
    void Delete(string correlationId);
    IEnumerable<IRequest> List(string contactId);
    IResult Retrieve(string correlationId);
  }
}
