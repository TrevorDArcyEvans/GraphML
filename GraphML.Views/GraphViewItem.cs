using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace GraphML.Views
{
  public abstract class GraphViewItem : OwnedItem
  {
    [Required]
    [JsonProperty(nameof(GraphItemId))]
    public Guid GraphItemId { get; set; } = Guid.NewGuid();

    public GraphViewItem() :
      base()
    {
    }

    public GraphViewItem(Guid graphViewId, string name) :
      base(graphViewId, name)
    {
    }

    public GraphViewItem(Guid graphViewId, Guid graphItemId) :
      base(graphViewId, nameof(GraphViewItem))
    {
      GraphItemId = graphItemId;
    }
  }
}
