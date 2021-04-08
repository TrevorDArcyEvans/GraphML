using System.Collections.Generic;

namespace GraphML.Datastore.Database.Importer
{
  public sealed class ImportSpecification
  {
    public string Organisation { get; set; }
    public string RepositoryManager { get; set; }
    public string Repository { get; set; }

    public string DataFile { get; set; }
    public bool HasHeaderRecord { get; set; }

    public int SourceNodeColumn { get; set; } = 0;
    public int TargetNodeColumn { get; set; } = 1;

    public List<NodeItemAttributeImportDefinition> NodeItemAttributeImportDefinitions { get; set; } = new List<NodeItemAttributeImportDefinition>();
    public List<EdgeItemAttributeImportDefinition> EdgeItemAttributeImportDefinitions { get; set; } = new List<EdgeItemAttributeImportDefinition>();
  }
}
