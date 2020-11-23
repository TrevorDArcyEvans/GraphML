using System.Threading.Tasks;
using GraphML.API.Server;

namespace GraphML.Interfaces.Server
{
  public interface IContactServer : IOwnedItemServerBase<Contact>
  {
    Task<Contact> ByEmail(string email);
  }
}
