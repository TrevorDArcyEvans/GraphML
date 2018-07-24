using Dapper.Contrib.Extensions;

namespace GraphML
{
  [Table(nameof(Node))]
  public sealed class Node : AttributedItem
  {
    public Node() :
      base()
    {
    }

    public Node(string ownerId, string name) :
      base(ownerId, name)
    {
    }
  }
}
