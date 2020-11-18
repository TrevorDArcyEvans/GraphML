using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace GraphML
{
  public abstract class OwnedItem : Item
  {
    /// <summary>
    /// Unique identifier of owner
    /// </summary>
    [Required]
    [JsonProperty(nameof(OwnerId))]
    public Guid OwnerId { get; set; } = Guid.Empty;

    public OwnedItem() :
      base()
    {
    }

    public OwnedItem(Guid ownerId, string name) :
      base(name)
    {
      OwnerId = ownerId;
    }
  }
}