using Dapper.Contrib.Extensions;
using GraphML.Utils;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GraphML
{
  [Table(nameof(ItemAttribute))]
  public sealed class ItemAttribute : OwnedItem
  {
    [Required]
    [JsonProperty(nameof(OwnerType))]
    public string OwnerType { get; set; }

    [Required]
    [JsonProperty(nameof(DataType))]
    public string DataType { get; set; }

    [JsonProperty(nameof(DataValueAsString))]
    public string DataValueAsString { get; set; } = string.Empty;

    public ItemAttribute() :
      base()
    {
    }

    public ItemAttribute(string ownerType, string ownerId, string name, string dataType) :
      base(ownerId, name)
    {
      OwnerType = ownerType.ThrowIfNullOrWhiteSpace(nameof(ownerType));
      DataType = dataType.ThrowIfNullOrWhiteSpace(nameof(dataType));
    }
  }
}
