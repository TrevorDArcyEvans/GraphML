namespace GraphML.Interfaces
{
  public interface IContactLogic : IOwnedLogic<Contact>
  {
    Contact ByEmail(string email);
  }
}
