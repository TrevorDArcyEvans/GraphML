using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IResultServer : IServerBase
  {
    Task Delete(Guid correlationId);
    Task<IEnumerable<IRequest>> ByContact(Guid contactId);
    Task<IEnumerable<IRequest>> ByOrganisation(Guid orgId);
    Task<IResult> Retrieve(Guid correlationId);
  }
}
