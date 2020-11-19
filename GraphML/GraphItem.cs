using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace GraphML
{
  public abstract class GraphItem : OwnedItem
  {
    [Write(false)]
    public Guid GraphId
    {
      get => OwnerId;
      set => OwnerId = value;
    }
    
    [Required]
    [JsonProperty(nameof(RepositoryItemId))]
    public Guid RepositoryItemId { get; set; }

    protected GraphItem() :
      base()
    {
    }

    protected GraphItem(Guid graph, Guid repositoryItem, string name) :
      base(graph, name)
    {
      RepositoryItemId = repositoryItem;
    }
  }
}
