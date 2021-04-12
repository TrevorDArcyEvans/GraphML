using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GraphML.Datastore.Database.Importer
{
  public sealed class ImportSpecification
  {
    [Required]
    [JsonProperty(nameof(Organisation))]
    public string Organisation { get; set; }

    [Required]
    [JsonProperty(nameof(RepositoryManager))]
    public string RepositoryManager { get; set; }

    [Required]
    [JsonProperty(nameof(Repository))]
    public string Repository { get; set; }

    [Required]
    [JsonProperty(nameof(DataFile))]
    public string DataFile { get; set; }

    [Required]
    [JsonProperty(nameof(HasHeaderRecord))]
    public bool HasHeaderRecord { get; set; }

    [Required]
    [JsonProperty(nameof(SourceNodeColumn))]
    public int SourceNodeColumn { get; set; } = 0;

    [Required]
    [JsonProperty(nameof(TargetNodeColumn))]
    public int TargetNodeColumn { get; set; } = 1;

    [Required]
    [JsonProperty(nameof(NodeItemAttributeImportDefinitions))]
    public List<NodeItemAttributeImportDefinition> NodeItemAttributeImportDefinitions { get; set; } = new List<NodeItemAttributeImportDefinition>();

    [Required]
    [JsonProperty(nameof(EdgeItemAttributeImportDefinitions))]
    public List<EdgeItemAttributeImportDefinition> EdgeItemAttributeImportDefinitions { get; set; } = new List<EdgeItemAttributeImportDefinition>();
  }
}
