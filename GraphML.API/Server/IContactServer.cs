namespace GraphML.API.Server
{
  public interface IContactServer : IOwnedItemServerBase<Contact>
  {
    Contact ByEmail(string email);
  }
}
