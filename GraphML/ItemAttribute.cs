using GraphML.Utils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace GraphML
{
  /// <summary>
  /// Additional data information attached to an Item
  /// </summary>
  public abstract class ItemAttribute : OwnedItem
  {
    /// <summary>
    /// Underlying data type
    /// Used for deserialisation
    /// </summary>
    [Required]
    [JsonProperty(nameof(DataType))]
    public string DataType { get; set; }

    /// <summary>
    /// Data value, serialised as a JSON string
    /// </summary>
    [JsonProperty(nameof(DataValueAsString))]
    public string DataValueAsString { get; set; } = string.Empty;

    public ItemAttribute() :
      base()
    {
    }

    public ItemAttribute(Guid owner, Guid orgId, string name, string dataType) :
      base(owner, orgId, name)
    {
      DataType = dataType.ThrowIfNullOrWhiteSpace(nameof(dataType));
    }
  }
}
