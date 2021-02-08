using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IRoleLogic : ILogic<Role>
  {
    IEnumerable<Role> ByContactId(Guid id);
    IEnumerable<Role> GetAll();
  }
}
