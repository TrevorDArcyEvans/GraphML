using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Datastore.Database.Importer;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace GraphML.API.Server
{
  public sealed class ImporterServer : ServerBase, IImporterServer
  {
    public ImporterServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<ImporterServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/Importer";

    public async Task Import(ImportSpecification importSpec, byte[] fileBytes, string fileName)
    {
      var form = new MultipartFormDataContent();
      var stream = new MemoryStream(fileBytes);
      var content = new StreamContent(stream);
      content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
      {
        // BEWARE - names have to match API parameter names!
        Name = "file",
        FileName = fileName
      };
      form.Add(content);

      // BEWARE - names have to match API parameter names!
      var json = JsonConvert.SerializeObject(importSpec);
      form.Headers.Add("importSpec", json);

      var path = Url.Combine(ResourceBase, nameof(ImporterController.Import));
      var resp = await _client.PostAsync(path, form);
    }
  }
}
