using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public interface IContactServer : IOwnedItemServerBase<Contact>
  {
    Task<Contact> ByEmail(string email);
  }
}
