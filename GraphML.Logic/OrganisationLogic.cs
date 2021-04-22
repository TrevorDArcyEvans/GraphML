using FluentValidation;
using System.Linq;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace GraphML.Logic
{
  public sealed class OrganisationLogic : LogicBase<Organisation>, IOrganisationLogic
  {
    private readonly IOrganisationDatastore _orgDatastore;

    public OrganisationLogic(
      IHttpContextAccessor context,
      IOrganisationDatastore datastore,
      IOrganisationValidator validator,
      IOrganisationFilter filter) :
      base(context, datastore, validator, filter)
    {
      _orgDatastore = datastore;
    }

    public PagedDataEx<Organisation> GetAll(int pageIndex, int pageSize, string searchTerm)
    {
      var valRes = _validator.Validate(new Organisation(), options => options.IncludeRuleSets(nameof(IOrganisationLogic.GetAll)));
      if (valRes.IsValid)
      {
        var pdex = _orgDatastore.GetAll(pageIndex, pageSize, searchTerm);
        var filtered = _filter.Filter(pdex.Items);
        return new PagedDataEx<Organisation>
        {
          TotalCount = pdex.TotalCount,
          Items = filtered.ToList()
        };
      }

      return new PagedDataEx<Organisation>();
    }
  }
}
