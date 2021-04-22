using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IOrganisationServer : IItemServerBase<Organisation>
  {
    Task<IEnumerable<Organisation>> GetAll(int pageIndex,	int pageSize, string searchTerm);
  }
}
