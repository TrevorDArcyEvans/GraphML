using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IContactsRolesDatastore
  {
    IEnumerable<Role> ByContact(Guid contact);
    IEnumerable<Contact> ByRole(Guid role);
  }
}
