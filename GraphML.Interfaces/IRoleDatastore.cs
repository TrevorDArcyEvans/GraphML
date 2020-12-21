using System.Collections.Generic;

namespace GraphML.Interfaces
{
    public interface IRoleDatastore : IDatastore<Role>
    {
        IEnumerable<Role> ByContactId(string id);
        IEnumerable<Role> GetAll();
    }
}
