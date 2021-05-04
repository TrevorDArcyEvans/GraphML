using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using GraphML.Common;

namespace GraphML.Logic
{
  public sealed class RoleLogic : LogicBase<Role>, IRoleLogic
  {
    private readonly IRoleDatastore _roleDatastore;
    private readonly IContactDatastore _contactDatastore;

    public RoleLogic(
      IHttpContextAccessor context,
      IRoleDatastore datastore,
      IRoleValidator validator,
      IRoleFilter filter,
      IContactDatastore contactDatastore) :
      base(context, datastore, validator, filter)
    {
      _roleDatastore = datastore;
      _contactDatastore = contactDatastore;
    }

    public IEnumerable<Role> ByContactId(Guid id)
    {
      var valRes = _validator.Validate(new Role(), options => options.IncludeRuleSets(nameof(IRoleLogic.ByContactId)));
      if (valRes.IsValid)
      {
        return _filter.Filter(_roleDatastore.ByContactId(id));
      }

      return Enumerable.Empty<Role>();
    }

    public IEnumerable<Role> GetAll()
    {
      var valRes = _validator.Validate(new Role(), options => options.IncludeRuleSets(nameof(IRoleLogic.GetAll)));
      if (valRes.IsValid)
      {
        return _filter.Filter(_roleDatastore.GetAll());
      }

      return Enumerable.Empty<Role>();
    }

    public IEnumerable<Role> Get()
    {
      var valRes = _validator.Validate(new Role(), options => options.IncludeRuleSets(nameof(IRoleLogic.Get)));
      if (valRes.IsValid)
      {
        var email = _context.Email();
        var contact = _contactDatastore.ByEmail(email);
        if (contact is null)
        {
          return Enumerable.Empty<Role>();
        }

        return _filter.Filter(_roleDatastore.ByContactId(contact.Id));
      }

      return Enumerable.Empty<Role>();
    }
  }
}
