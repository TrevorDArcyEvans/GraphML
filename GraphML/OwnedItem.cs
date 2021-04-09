using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace GraphML
{
  /// <summary>
  /// Something which has an immediate owner, other than an <see cref="Organisation"/>
  /// </summary>
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

    public OwnedItem(Guid owner, Guid org, string name) :
      base(org, name)
    {
      OwnerId = owner;
    }
  }
}
