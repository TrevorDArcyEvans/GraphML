using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IContactServer : IOwnedItemServerBase<Contact>
  {
    Task<Contact> ByEmail(string email);
  }
}
