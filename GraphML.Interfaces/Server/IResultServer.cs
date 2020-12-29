using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IResultServer : IServerBase
  {
    Task Delete(string correlationId);
    Task<IEnumerable<IRequest>> ByContact(string contactId);
    Task<IEnumerable<IRequest>> ByOrganisation(string orgId);
    Task<IResult> Retrieve(string correlationId);
  }
}
