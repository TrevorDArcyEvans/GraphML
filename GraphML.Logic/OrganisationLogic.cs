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

    public IEnumerable<Organisation> GetAll()
    {
      var valRes = _validator.Validate(new Organisation(), ruleSet: nameof(IOrganisationLogic.GetAll));
      if (valRes.IsValid)
      {
        return _filter.Filter(_orgDatastore.GetAll());
      }

      return Enumerable.Empty<Organisation>();
    }
  }
}
