using System;
using System.IO;
using FluentValidation;
using GraphML.Datastore.Database.Importer;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class ImporterLogic : IImporterLogic
  {
    private readonly ILogger<ImporterLogic> _logger;
    private readonly IImporterValidator _validator;

    public ImporterLogic(
      ILogger<ImporterLogic> logger,
      IImporterValidator validator)
    {
      _logger = logger;
      _validator = validator;
    }
    
    public void Import(
      ImportSpecification importSpec, 
      IConfiguration config, 
      Stream stream, 
      Action<string> logInfoAction = null)
    {
      var valRes = _validator.Validate(importSpec, options => options.IncludeRuleSets(nameof(IImporterLogic.Import)));
      if (!valRes.IsValid)
      {
        _logger.LogError(valRes.ToString());
        return;
      }

      var importer = new Importer(importSpec, config, stream, logInfoAction);
      importer.Run();
    }
  }
}
