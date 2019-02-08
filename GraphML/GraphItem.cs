using Newtonsoft.Json;
using System;

namespace GraphML
{
  public abstract class GraphItem : AttributedItem
  {
    [JsonProperty(nameof(NextId))]
    public string NextId { get; set; }

    protected GraphItem() :
      base()
    {
    }

    protected GraphItem(string ownerId, string name) :
      base(ownerId, name)
    {
    }
  }
}
