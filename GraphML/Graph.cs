using Newtonsoft.Json;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(Graph))]
  public sealed class Graph : AttributedItem
  {
    [JsonProperty(nameof(Directed))]
    public bool Directed { get; set; } = true;

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
