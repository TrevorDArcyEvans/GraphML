﻿using GraphML.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public interface IResultServer : IServerBase
  {
    Task Delete(string correlationId);
    Task<IEnumerable<IRequest>> List(string contactId);
    Task<IResult> Retrieve(string correlationId);
  }
}
