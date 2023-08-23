using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using IResult = GraphML.Interfaces.IResult;

namespace GraphML.Logic
{
  public sealed class ResultLogic : IResultLogic
  {
    private readonly IHttpContextAccessor _context;
    private readonly ILogger<ResultLogic> _logger;
    private readonly IResultDatastore _datastore;
    private readonly IResultValidator _validator;

    public ResultLogic(
      IHttpContextAccessor context,
      ILogger<ResultLogic> logger,
      IResultDatastore datastore,
      IResultValidator validator)
    {
      _context = context;
      _logger = logger;
      _datastore = datastore;
      _validator = validator;
    }

    public void Create(IRequest request, string resultJson)
    {
      // called by Analysis.Server to store results
      var valRes = _validator.Validate(request.Contact.Id, options => options.IncludeRuleSets(nameof(IResultLogic.Create)));
      if (!valRes.IsValid)
      {
        _logger.LogError(valRes.ToString());
        return;
      }

      _datastore.Create(request, resultJson);
    }

    public void Delete(Guid correlationId)
    {
      // called by user
      var valRes = _validator.Validate(correlationId, options => options.IncludeRuleSets(nameof(IResultLogic.Delete)));
      if (!valRes.IsValid)
      {
        _logger.LogError(valRes.ToString());
        return;
      }

      _datastore.Delete(correlationId);
    }

    public IEnumerable<IRequest> ByContact(Guid contactId)
    {
      // called by user
      var valRes = _validator.Validate(contactId, options => options.IncludeRuleSets(nameof(IResultLogic.ByContact)));
      if (!valRes.IsValid)
      {
        _logger.LogError(valRes.ToString());
        return Enumerable.Empty<IRequest>();
      }

      return _datastore.ByContact(contactId);
    }

    public IEnumerable<IRequest> ByOrganisation(Guid orgId)
    {
      // called by user
      var valRes = _validator.Validate(orgId, options => options.IncludeRuleSets(nameof(IResultLogic.ByOrganisation)));
      if (!valRes.IsValid)
      {
        _logger.LogError(valRes.ToString());
        return Enumerable.Empty<IRequest>();
      }

      return _datastore.ByOrganisation(orgId);
    }

    public IEnumerable<IRequest> ByGraph(Guid graphId)
    {
      // called by user
      var valRes = _validator.Validate(graphId, options => options.IncludeRuleSets(nameof(IResultLogic.ByGraph)));
      if (!valRes.IsValid)
      {
        _logger.LogError(valRes.ToString());
        return Enumerable.Empty<IRequest>();
      }

      return _datastore.ByGraph(graphId);
    }

    public IRequest ByCorrelation(Guid corrId)
    {
      // called by user
      var valRes = _validator.Validate(corrId, options => options.IncludeRuleSets(nameof(IResultLogic.ByCorrelation)));
      if (!valRes.IsValid)
      {
        _logger.LogError(valRes.ToString());
        return null;
      }

      return _datastore.ByCorrelation(corrId);
    }

    public IResult Retrieve(Guid correlationId)
    {
      // called by user
      var valRes = _validator.Validate(correlationId, options => options.IncludeRuleSets(nameof(IResultLogic.Retrieve)));
      if (!valRes.IsValid)
      {
        _logger.LogError(valRes.ToString());
        return null;
      }

      return _datastore.Retrieve(correlationId);
    }
  }
}
