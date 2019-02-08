using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Contact))]
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
