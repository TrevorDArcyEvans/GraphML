using System.Threading.Tasks;
using GraphML.Datastore.Database.Importer;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
      // [FromForm] [Required] ImportSpecification importSpec
      // [Required] IFormFile file
      throw new System.NotImplementedException();
    }
  }
}
