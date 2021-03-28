using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
    public interface IRepositoryItemServer<T> : IOwnedItemServerBase<T>
    {
        Task<IEnumerable<T>> GetParents(T entity, int pageIndex, int pageSize);
    }
}