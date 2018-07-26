using GraphML.Utils;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GraphML
{
  public abstract class ItemAttribute : OwnedItem
  {
    [Required]
    [JsonProperty(nameof(DataType))]
    public string DataType { get; set; }

    [JsonProperty(nameof(DataValueAsString))]
    public string DataValueAsString { get; set; } = string.Empty;

    public ItemAttribute() :
      base()
    {
    }

    public ItemAttribute(string ownerId, string name, string dataType) :
      base(ownerId, name)
    {
      DataType = dataType.ThrowIfNullOrWhiteSpace(nameof(dataType));
    }
  }
}
