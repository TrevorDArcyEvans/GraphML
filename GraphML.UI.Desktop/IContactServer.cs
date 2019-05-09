namespace GraphML.UI.Desktop
{
  public interface IContactServer : IOwnedItemServerBase<Contact>
  {
    Contact ByEmail(string email);
  }
}
