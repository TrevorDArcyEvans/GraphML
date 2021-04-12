using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GraphML.Datastore.Database.Importer
{
  public sealed class NodeItemAttributeImportDefinition : ItemAttributeImportDefinition
  {
    [Required]
    [JsonProperty(nameof(ApplyTo))]
    public ApplyTo ApplyTo { get; set; }
  }
}
