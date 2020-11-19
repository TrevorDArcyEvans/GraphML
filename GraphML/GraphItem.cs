using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace GraphML
{
  public abstract class GraphItem : OwnedItem
  {
    [JsonProperty(nameof(NextId))]
    public string NextId { get; set; }

    [Write(false)]
    public Guid GraphId
    {
      get => OwnerId;
      set => OwnerId = value;
    }

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
