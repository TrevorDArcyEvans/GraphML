using System;
using System.Collections.Generic;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphML.Logic
{
  public sealed class ResultLogic : IResultLogic
  {
    private readonly IHttpContextAccessor _context;
    private readonly IResultDatastore _datastore;

    public ResultLogic(
      IHttpContextAccessor context,
      IResultDatastore datastore)
    {
      _context = context;
      _datastore = datastore;
    }

    public void Create(IRequest request, string resultJson)
    {
      // TODO   validation
      _datastore.Create(request, resultJson);
    }

    public void Delete(string correlationId)
    {
      // TODO   validation
      _datastore.Delete(correlationId);
    }

    public IEnumerable<IRequest> List(string contactId)
    {
      // TODO   validation
      return _datastore.List(contactId);
    }

    public IResult Retrieve(string correlationId)
    {
      // TODO   validation
      return _datastore.Retrieve(correlationId);
    }
  }
}
