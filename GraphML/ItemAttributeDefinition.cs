using System;
using System.ComponentModel.DataAnnotations;
using GraphML.Utils;
using Newtonsoft.Json;

namespace GraphML
{
  /// <summary>
  /// <para>Defines shape (name and data type) of information in an <see cref="ItemAttribute"/></para>
  /// <para>ItemAttributeDefinition are held at <see cref="RepositoryManager"/> level so they can be shared across <see cref="Repository"/>.</para>
  /// </summary>
  public abstract class ItemAttributeDefinition : OwnedItem
  {
    /// <summary>
    /// Underlying data type
    /// Used for deserialisation
    /// </summary>
    [Required]
    [JsonProperty(nameof(DataType))]
    public string DataType { get; set; }

    public ItemAttributeDefinition() :
      base()
    {
    }

    public ItemAttributeDefinition(Guid repoMgr, Guid orgId, string name, string dataType) :
      base(repoMgr, orgId, name)
    {
      DataType = dataType.ThrowIfNullOrWhiteSpace(nameof(dataType));
    }
  }
}
