using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public interface IOrganisationServer : IItemServerBase<Organisation>
  {
    Task<IEnumerable<Organisation>> GetAll();
  }
}
