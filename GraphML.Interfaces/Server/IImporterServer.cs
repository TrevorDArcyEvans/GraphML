using System.Threading.Tasks;
using GraphML.Datastore.Database.Importer;
using Microsoft.AspNetCore.Http;

namespace GraphML.Interfaces.Server
{
  public interface IImporterServer
  {
    Task Import(ImportSpecification importSpec, IFormFile file);
  }
}
