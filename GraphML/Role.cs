using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Role))]
  public sealed class Role : Item
  {
    public Role() :
      base()
    {
    }

    public Role(Guid orgId, string name) :
      base(orgId, name)
    {
    }
  }
}
