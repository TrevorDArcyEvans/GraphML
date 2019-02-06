using Dapper.Contrib.Extensions;

namespace GraphML
{
  [Table(nameof(Contact))]
  public sealed class Contact : OwnedItem
  {
    public Contact() :
      base()
    {
    }

    public Contact(string orgId, string email) :
      base(orgId, email)
    {
    }
  }
}
