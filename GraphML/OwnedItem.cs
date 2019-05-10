using GraphML.Utils;
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
    public string OwnerId { get; set; } = Guid.Empty.ToString();

    public OwnedItem() :
      base()
    {
    }

    public OwnedItem(string ownerId, string name) :
      base(name)
    {
      OwnerId = ownerId.ThrowIfNullOrWhiteSpace(nameof(ownerId));
    }
  }
}