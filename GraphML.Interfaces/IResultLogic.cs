﻿using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IResultLogic
  {
    void Create(IRequest request, string resultJson);

    /// <summary>
    /// List stored and completed requests for this person
    /// </summary>
    /// <param name="contactId"></param>
    /// <returns>list of CorrelationIds</returns>
    IEnumerable<IRequest> List(string contactId);

    IResult Retrieve(string correlationId);
    void Delete(string correlationId);
  }
}