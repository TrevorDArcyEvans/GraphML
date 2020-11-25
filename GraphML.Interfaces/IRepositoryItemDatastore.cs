using System.Collections.Generic;

namespace GraphML.Interfaces
{
    public interface IRepositoryItemDatastore<T> : IOwnedDatastore<T>
    {
        IEnumerable<T> GetParents(T entity, int pageIndex, int pageSize);
    }
}