using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

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
      // called by Analysis.Server to store results
      var valRes = _validator.Validate(request.Contact.Id, options => options.IncludeRuleSets(nameof(IResultLogic.Create)));
      if (!valRes.IsValid)
      {
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
        return Enumerable.Empty<IRequest>();
      }

      return _datastore.ByContact(contactId);
    }

    public IEnumerable<IRequest> ByOrganisation(Guid orgid)
    {
      // called by user
      var valRes = _validator.Validate(orgid, options => options.IncludeRuleSets(nameof(IResultLogic.ByOrganisation)));
      if (!valRes.IsValid)
      {
        return Enumerable.Empty<IRequest>();
      }

      return _datastore.ByOrganisation(orgid);
    }

    public IResult Retrieve(Guid correlationId)
    {
      // called by user
      var valRes = _validator.Validate(correlationId, options => options.IncludeRuleSets(nameof(IResultLogic.Retrieve)));
      if (!valRes.IsValid)
      {
        return null;
      }

      return _datastore.Retrieve(correlationId);
    }
  }
}
