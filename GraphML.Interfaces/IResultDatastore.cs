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
    IEnumerable<IRequest> List(string contactId);

    string Retrieve(string correlationId);
    void Delete(string correlationId);
  }
}
