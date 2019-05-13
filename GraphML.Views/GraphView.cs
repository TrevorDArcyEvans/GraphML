using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GraphML.Views
{
  public abstract class GraphView : OwnedItem
  {
    [Required]
    [JsonProperty(nameof(ViewType))]
    public string ViewType { get; set; }

    public GraphView() :
      base()
    {
    }

    public GraphView(string graphId, string name) :
      base(graphId, name)
    {
    }
  }
}
