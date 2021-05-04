using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IResultDatastore
  {
    void Create(IRequest request, string resultJson);

    /// <summary>
    /// List stored and completed requests for this <see cref="Contact"/>
    /// </summary>
    /// <param name="contactId"></param>
    /// <returns>list of CorrelationIds</returns>
    IEnumerable<IRequest> ByContact(Guid contactId);

    /// <summary>
    /// List stored and completed requests for this <see cref="Organisation"/>
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns>list of CorrelationIds</returns>
    IEnumerable<IRequest> ByOrganisation(Guid orgId);

    /// <summary>
    /// List stored and completed requests for this <see cref="Graph"/>
    /// </summary>
    /// <param name="graphId"></param>
    /// <returns>list of CorrelationIds</returns>
    IEnumerable<IRequest> ByGraph(Guid graphId);

    IRequest ByCorrelation(Guid corrId);

    IResult Retrieve(Guid correlationId);
    void Delete(Guid correlationId);
  }
}
