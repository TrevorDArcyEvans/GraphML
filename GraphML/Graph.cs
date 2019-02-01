using System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Table(nameof(Graph))]
  public sealed class Graph : AttributedItem
  {
    public Graph() :
      base()
    {
    }

    public Graph(string ownerId, string name) :
      base(ownerId, name)
    {
    }
  }
}
