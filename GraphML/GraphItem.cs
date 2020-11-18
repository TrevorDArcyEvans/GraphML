using System;
using Newtonsoft.Json;

namespace GraphML
{
  public abstract class GraphItem : OwnedItem
  {
    [JsonProperty(nameof(NextId))]
    public string NextId { get; set; }

    protected GraphItem() :
      base()
    {
    }

    protected GraphItem(Guid graphId, string name) :
      base(graphId, name)
    {
    }
  }
}
