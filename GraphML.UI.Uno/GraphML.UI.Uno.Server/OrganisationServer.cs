using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphML.Interfaces.Server;

namespace GraphML.UI.Uno.Server
{
    public class OrganisationServer : IOrganisationServer
    {
        public Task<IEnumerable<Organisation>> ByIds(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Organisation>> Create(IEnumerable<Organisation> entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Organisation>> Delete(IEnumerable<Organisation> entity)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<IEnumerable<Organisation>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Organisation>> Update(IEnumerable<Organisation> entity)
        {
            throw new NotImplementedException();
        }
    }
}
