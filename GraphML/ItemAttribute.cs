using Newtonsoft.Json;
using System;

namespace GraphML
{
  /// <summary>
  /// Additional data information attached to an <see cref="Item"/>
  /// </summary>
  public abstract class ItemAttribute : OwnedItem
  {
    /// <summary>
    /// Data value, serialised as a JSON string
    /// </summary>
    [JsonProperty(nameof(DataValueAsString))]
    public string DataValueAsString { get; set; } = string.Empty;

    public ItemAttribute() :
      base()
    {
    }

    public ItemAttribute(Guid owner, Guid orgId, string name) :
      base(owner, orgId, name)
    {
    }
  }
}
