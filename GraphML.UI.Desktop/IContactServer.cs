namespace GraphML.UI.Desktop
{
  public interface IContactServer : IServerBase<Contact>
  {
    Contact ByEmail(string email);
  }
}
