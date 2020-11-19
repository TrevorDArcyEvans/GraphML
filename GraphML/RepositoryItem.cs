using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace GraphML
{
  public abstract class RepositoryItem : OwnedItem
  {
    [JsonProperty(nameof(NextId))]
    public Guid NextId { get; set; }

    [Write(false)]
    public Guid RepositoryId
    {
      get => OwnerId;
      set => OwnerId = value;
    }

    protected RepositoryItem() :
      base()
    {
    }

    protected RepositoryItem(Guid graph, string name) :
      base(graph, name)
    {
    }
  }
}
