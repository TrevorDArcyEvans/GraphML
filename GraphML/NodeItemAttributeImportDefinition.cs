using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GraphML.Datastore.Database.Importer
{
  public sealed class NodeItemAttributeImportDefinition : ItemAttributeImportDefinition
  {
    /// <summary>
    /// To what combination of nodes to apply a <see cref="NodeItemAttribute"/>:
    /// <list type="bullet">
    /// <item>SourceNode</item>
    /// <item>TargetNode</item>
    /// <item>BothNodes</item>
    /// </list>
    /// </summary>
    [Required]
    [JsonProperty(nameof(ApplyTo))]
    public ApplyTo ApplyTo { get; set; }
  }
}
