using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IOrganisationLogic : ILogic<Organisation>
  {
    PagedDataEx<Organisation> GetAll(int pageIndex, int pageSize, string searchTerm);
  }
}
