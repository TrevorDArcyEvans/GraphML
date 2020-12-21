using System.Collections.Generic;

namespace GraphML.Interfaces
{
    public interface IRoleLogic : ILogic<Role>
    {
        IEnumerable<Role> ByContactId(string id);
        IEnumerable<Role> GetAll();
    }
}
