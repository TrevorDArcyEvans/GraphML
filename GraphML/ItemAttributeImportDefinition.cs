using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

namespace GraphML.Datastore.Database.Importer
{
  public abstract class ItemAttributeImportDefinition
  {
    [Required]
    [JsonProperty(nameof(Name))]
    public string Name { get; set; }
    
    [Required]
    [JsonProperty(nameof(DataType))]
    public string DataType { get; set; }
    
    [Required]
    [JsonProperty(nameof(DateTimeFormat))]
    public string DateTimeFormat { get; set; }
    
    [Required]
    [JsonProperty(nameof(Columns))]
    public int[] Columns { get; set; } = Enumerable.Empty<int>().ToArray();
  }
}
