using System;
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

    public Contact(Guid orgId, string email) :
      base(orgId, email)
    {
    }
  }
}
