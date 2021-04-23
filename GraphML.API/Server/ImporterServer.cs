using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Datastore.Database.Importer;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
      var path = Url.Combine(ResourceBase, nameof(ImporterController.Import));
      var req = GetRequest(path);
      var json = JsonConvert.SerializeObject(importSpec);

      // Content-Type defaults to application/json but we are posting
      // form-data, so clear this header as it is incorrect
      req.Headers.Add("Content-Type", string.Empty);
      req.Method = HttpMethod.Post;
      ;

      // BEWARE - names have to match API parameter names!
      req.Headers.Add("importSpec", json);
      // TODO   req.AddFileBytes("file", fileBytes, fileName);

      var res = await GetRawResponse(req);
    }
  }
}
