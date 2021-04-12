using System.Threading.Tasks;
using GraphML.Datastore.Database.Importer;

namespace GraphML.Interfaces.Server
{
  public interface IImporterServer
  {
    Task Import(ImportSpecification importSpec, byte[] fileBytes, string fileName);
  }
}
