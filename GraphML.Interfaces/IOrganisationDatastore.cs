using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IOrganisationDatastore : IDatastore<Organisation>
  {
    PagedDataEx<Organisation> GetAll(int pageIndex, int pageSize, string searchTerm);
  }
}
