using Newtonsoft.Json;

namespace GraphML
{
  public abstract class GraphItem : AttributedItem
  {
    [JsonProperty(nameof(PreviousId))]
    public string PreviousId { get; set; }

    protected GraphItem() :
      base()
    {
    }

    protected GraphItem(string graphId, string name) :
      base(graphId, name)
    {
    }
  }
}
