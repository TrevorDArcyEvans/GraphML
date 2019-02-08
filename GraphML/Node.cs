using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Node))]
  public sealed class Node : GraphItem
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
