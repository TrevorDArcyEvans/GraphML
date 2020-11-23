using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.Interfaces.Server;

namespace GraphML.API.Server
{
  public interface IOrganisationServer : IItemServerBase<Organisation>
  {
    Task<IEnumerable<Organisation>> GetAll();
  }
}
