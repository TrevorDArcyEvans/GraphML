using System;
using System.IO;
using GraphML.Datastore.Database.Importer;
using Microsoft.Extensions.Configuration;

namespace GraphML.Interfaces
{
  public interface IImporterLogic
  {
    void Import(
      ImportSpecification importSpec,
      IConfiguration config,
      Stream stream,
      Action<string> logInfoAction = null);
  }
}
