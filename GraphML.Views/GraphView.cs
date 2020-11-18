using Newtonsoft.Json;
using System;
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

    public GraphView(Guid graphId, string name) :
      base(graphId, name)
    {
    }
  }
}
