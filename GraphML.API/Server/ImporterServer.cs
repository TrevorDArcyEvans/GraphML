using System.IO;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Datastore.Database.Importer;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace GraphML.API.Server
{
  public sealed class ImporterServer : ServerBase, IImporterServer
  {
    public ImporterServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<ImporterServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/Importer";

    public Task Import(ImportSpecification importSpec, IFormFile file)
    {
      var path = Url.Combine(ResourceBase, nameof(ImporterController.Import));
      var req = GetRequest(path);
      var json = JsonConvert.SerializeObject(importSpec);
      using var ms = new MemoryStream();

      file.CopyTo(ms);

      req.Method = Method.POST;

      // BEWARE - names have to match API parameter names!
      req.AddHeader("importSpec", json);
      req.AddFileBytes("file", ms.ToArray(), file.FileName);

      var res = GetRawResponse(req);
      throw new System.NotImplementedException();
    }
  }
}
