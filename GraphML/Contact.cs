using System;
using Dapper.Contrib.Extensions;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Contact))]
  public sealed class Contact : OwnedItem
  {
    [Write(false)]
    public Guid OrganisationId
    {
      get => OwnerId;
      set => OwnerId = value;
    }

    public Contact() :
      base()
    {
    }

    public Contact(Guid org, string email) :
      base(org, email)
    {
    }
  }
}
