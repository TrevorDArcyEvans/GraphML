using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphML.Datastore.Database.Importer;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace GraphML.UI.Web.Pages
{
  public partial class Importer
  {
    #region Parameters

    [Parameter]
    public string OrganisationName { get; set; }

    [Parameter]
    public string OrganisationId { get; set; }

    [Parameter]
    public string RepositoryManagerName { get; set; }

    [Parameter]
    public string RepositoryManagerId { get; set; }

    #endregion

    private string ImportSpec { get; set; }
    private string CurrentFileName { get; set; } = string.Empty;
    private IMatFileUploadEntry _currentFile;

    protected override void OnInitialized()
    {
      base.OnInitialized();
      
      ImportSpec = JsonConvert.SerializeObject(GetSampleImportSpecification(), new JsonSerializerSettings { Formatting = Formatting.Indented });
    }

    private void FilesReadyForContent(IMatFileUploadEntry[] files)
    {
      _currentFile = files.FirstOrDefault();
      if (_currentFile == null)
      {
        return;
      }

      CurrentFileName = _currentFile.Name;
    }

    private async Task DoImport()
    {
      var importSpecObj = JsonConvert.DeserializeObject<ImportSpecification>(ImportSpec);
      await using var ms = new MemoryStream();

      await _currentFile.WriteToStreamAsync(ms);
      await _importerServer.Import(importSpecObj, ms.ToArray(), CurrentFileName);

      _toaster.Add("Imported", MatToastType.Success, "Finished!");
    }

    private ImportSpecification GetSampleImportSpecification()
    {
      return new ImportSpecification
      {
        Organisation = OrganisationName,
        RepositoryManager = RepositoryManagerName,
        Repository = "Repository",
        NodeItemAttributeImportDefinitions = new List<NodeItemAttributeImportDefinition>
        {
          new()
        },
        EdgeItemAttributeImportDefinitions = new List<EdgeItemAttributeImportDefinition>
        {
          new()
        }
      };
    }
  }
}
