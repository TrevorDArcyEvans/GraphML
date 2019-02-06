using Dapper.Contrib.Extensions;

namespace GraphML
{
  [Table(nameof(Organisation))]
  public sealed class Organisation : Item
  {
    public Organisation() :
      base()
    {
    }

    public Organisation(string name) :
      base(name)
    {
    }
  }
}
