using GraphML.Utils;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GraphML.Views
{
  public abstract class GraphViewItem : OwnedItem
  {
    [Required]
    [JsonProperty(nameof(GraphItemId))]
    public string GraphItemId { get; set; }

    public GraphViewItem() :
      base()
    {
    }

    public GraphViewItem(string graphViewId, string graphItemId) :
      base(graphViewId, nameof(GraphViewItem))
    {
      GraphItemId = graphItemId.ThrowIfNullOrWhiteSpace(nameof(graphItemId));
    }
  }
}
