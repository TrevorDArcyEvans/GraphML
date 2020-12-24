using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace GraphML.Logic
{
  public sealed class ResultLogic : IResultLogic
  {
    private readonly IHttpContextAccessor _context;
    private readonly IResultDatastore _datastore;
    private readonly IResultValidator _validator;

    public ResultLogic(
      IHttpContextAccessor context,
      IResultDatastore datastore, 
      IResultValidator validator)
    {
      _context = context;
      _datastore = datastore;
      _validator = validator;
    }

    public void Create(IRequest request, string resultJson)
    {
      // TODO   validation
      // called by Analysis.Server to store results
      _datastore.Create(request, resultJson);
    }

    public void Delete(Guid correlationId)
    {
      // TODO   validation
      // called by user
      _datastore.Delete(correlationId);
    }

    public IEnumerable<IRequest> List(Guid contactId)
    {
      // TODO   validation
      // called by user
      return _datastore.List(contactId);
    }

    public IResult Retrieve(Guid correlationId)
    {
      // TODO   validation
      // called by user
      return _datastore.Retrieve(correlationId);
    }
  }
}
