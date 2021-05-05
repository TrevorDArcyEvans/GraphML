using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IResultServer
  {
    Task Delete(Guid correlationId);
    Task<IEnumerable<IRequest>> ByContact(Guid contactId);
    Task<IEnumerable<IRequest>> ByOrganisation(Guid orgId);
    Task<IEnumerable<IRequest>> ByGraph(Guid graphId);
    Task<IRequest> ByCorrelation(Guid correlationId);
    Task<IResult> Retrieve(Guid correlationId);
  }
}
