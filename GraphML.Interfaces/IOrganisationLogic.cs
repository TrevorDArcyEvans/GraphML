using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IOrganisationLogic : ILogic<Organisation>
  {
    IEnumerable<Organisation> GetAll();
  }
}
