using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using GraphML.API.Attributes;
using GraphML.Datastore.Database.Importer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GraphML.Interfaces;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Import a CSV or TSV into a Repository
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class ImporterController : ControllerBase
  {
    private readonly IImporterLogic _logic;
    private readonly IConfiguration _config;
    private readonly ILogger<ImporterController> _logger;

    public ImporterController(
      IImporterLogic logic,
      IConfiguration config,
      ILogger<ImporterController> logger)
    {
      _logic = logic;
      _config = config;
      _logger = logger;
    }

    /// <summary>
    /// Import a CSV or TSV file into a new or existing Repository
    /// according to the specification
    /// </summary>
    /// <param name="importSpec">Import specification</param>
    /// <param name="file">CSV or TSV file</param>
    /// <response code="200">Success</response>
    /// <response code="422">Something wrong with import specification</response>
    [HttpPost]
    [Route(nameof(Import))]
    [ValidateModelState]
    [DisableRequestSizeLimit]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.UnprocessableEntity)]
    public async Task<ActionResult> Import(
      [FromForm] [Required] ImportSpecification importSpec,
      [Required] IFormFile file)
    {
      await using var stream = file.OpenReadStream();

      // In this scenario, IFormFile is single source of truth as no access to client file system
      importSpec.DataFile = file.FileName;

      _logic.Import(importSpec, _config, stream, msg => _logger.LogInformation(msg ?? Environment.NewLine));

      return Ok();
    }
  }
}
