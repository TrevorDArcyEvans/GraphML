using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GraphML.Datastore.Database.Importer
{
  public sealed class ImportSpecification
  {
    /// <summary>
    /// Name of <see cref="Organisation"/>.<br/>
    /// <remarks>
    /// This Organisation must already exist.
    /// </remarks>
    /// </summary>
    [Required]
    [JsonProperty(nameof(Organisation))]
    public string Organisation { get; set; }

    /// <summary>
    /// Name of <see cref="RepositoryManager"/>.<br/>
    /// <remarks>
    /// This RepositoryManager must already exist.
    /// </remarks>
    /// </summary>
    [Required]
    [JsonProperty(nameof(RepositoryManager))]
    public string RepositoryManager { get; set; }

    /// <summary>
    /// Name of <see cref="Repository"/>.<br/>
    /// <remarks>
    /// If the Repository does not exist, it will be created.
    /// </remarks>
    /// </summary>
    [Required]
    [JsonProperty(nameof(Repository))]
    public string Repository { get; set; }

    /// <summary>
    /// Relative or absolute path to csv or tsv file containing data to import.
    /// </summary>
    [Required]
    [JsonProperty(nameof(DataFile))]
    public string DataFile { get; set; }

    /// <summary>
    /// Whether or not first line of <see cref="DataFile"/> is a header and should be ignored.
    /// </summary>
    [Required]
    [JsonProperty(nameof(HasHeaderRecord))]
    public bool HasHeaderRecord { get; set; }

    /// <summary>
    /// Zero based index of column containing source nodes.
    /// </summary>
    [Required]
    [JsonProperty(nameof(SourceNodeColumn))]
    public int SourceNodeColumn { get; set; } = 0;

    /// <summary>
    /// Zero based index of column containing target nodes.
    /// </summary>
    [Required]
    [JsonProperty(nameof(TargetNodeColumn))]
    public int TargetNodeColumn { get; set; } = 1;

    /// <summary>
    /// Collection of <see cref="NodeItemAttributeImportDefinition"/>.<br/>
    /// <remarks>
    /// If not required, just specify an empty collection.<br/>
    /// Will be created if it does not exist.<br/>
    /// If it does exist, it must be same data type.<br/>
    /// </remarks>
    /// </summary>
    [Required]
    [JsonProperty(nameof(NodeItemAttributeImportDefinitions))]
    public List<NodeItemAttributeImportDefinition> NodeItemAttributeImportDefinitions { get; set; } = new List<NodeItemAttributeImportDefinition>();

    /// <summary>
    /// Collection of <see cref="EdgeItemAttributeImportDefinition"/>.<br/>
    /// <remarks>
    /// If not required, just specify an empty collection.<br/>
    /// Will be created if it does not exist.<br/>
    /// If it does exist, it must be same data type.<br/>
    /// </remarks>
    /// </summary>
    [Required]
    [JsonProperty(nameof(EdgeItemAttributeImportDefinitions))]
    public List<EdgeItemAttributeImportDefinition> EdgeItemAttributeImportDefinitions { get; set; } = new List<EdgeItemAttributeImportDefinition>();
  }
}
