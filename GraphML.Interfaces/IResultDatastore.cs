using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IResultDatastore
  {
    void Create(IRequest request, string resultJson);

    /// <summary>
    /// List stored and completed requests for this person
    /// </summary>
    /// <param name="contactId"></param>
    /// <returns>list of CorrelationIds</returns>
    IEnumerable<IRequest> ByContact(Guid contactId);

    /// <summary>
    /// List stored and completed requests for this organisation
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns>list of CorrelationIds</returns>
    IEnumerable<IRequest> ByOrganisation(Guid orgId);

    IRequest ByCorrelation(Guid corrId);

    IResult Retrieve(Guid correlationId);
    void Delete(Guid correlationId);
  }
}
