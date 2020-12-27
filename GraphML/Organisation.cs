using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Organisation))]
  public sealed class Organisation : Item
  {
    public Organisation() :
      base()
    {
    }

    public Organisation(Guid org, string name) :
      base(org, name)
    {
    }
  }
}
