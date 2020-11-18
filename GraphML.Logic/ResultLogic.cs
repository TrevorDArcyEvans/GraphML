using GraphML.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

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

    public void Delete(Guid correlationId)
    {
      // TODO   validation
      _datastore.Delete(correlationId);
    }

    public IEnumerable<IRequest> List(Contact contact)
    {
      // TODO   validation
      return _datastore.List(contact);
    }

    public IResult Retrieve(Guid correlationId)
    {
      // TODO   validation
      return _datastore.Retrieve(correlationId);
    }
  }
}
