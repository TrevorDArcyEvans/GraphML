using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IRoleDatastore : IItemDatastore<Role>
  {
    IEnumerable<Role> ByContactId(Guid id);
    IEnumerable<Role> GetAll();
  }
}
