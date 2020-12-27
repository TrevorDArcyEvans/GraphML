using System;
using Dapper.Contrib.Extensions;
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

    public Contact(Guid org, string email) :
      base(org, org, email)
    {
    }
  }
}
