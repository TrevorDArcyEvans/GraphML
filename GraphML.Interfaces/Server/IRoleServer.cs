using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IRoleServer : IItemServerBase<Role>
  {
    Task<IEnumerable<Role>> ByContactId(Guid id);
    Task<IEnumerable<Role>> GetAll();
    Task<IEnumerable<Role>> Get();
  }
}
