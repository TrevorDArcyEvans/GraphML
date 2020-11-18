using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IResultLogic
  {
    void Create(IRequest request, string resultJson);

    /// <summary>
    /// List stored and completed requests for this person
    /// </summary>
    /// <param name="contact"></param>
    /// <returns>list of CorrelationIds</returns>
    IEnumerable<IRequest> List(Contact contact);

    IResult Retrieve(Guid correlationId);
    void Delete(Guid correlationId);
  }
}
