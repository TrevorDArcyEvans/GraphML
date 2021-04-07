using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using GraphML.API.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

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
    /// <summary>
    /// Import a CSV or TSV file into a new or existing Repository
    /// according to the specification
    /// </summary>
    /// <param name="importSpec">Import specification</param>
    /// <param name="file">CSV or TSV file</param>
    /// <response code="200">Success</response>
    /// <response code="404">Something not found</response>
    [HttpPost]
    [Route(nameof(Import))]
    [ValidateModelState]
    [DisableRequestSizeLimit]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public async Task<ActionResult> Import(
      [Required] string importSpec,
      [Required] IFormFile file)
    {
      // TODO     check importSpec for validity
      // TODO     import data
      var assyPath = Assembly.GetExecutingAssembly().Location;
      var assyDir = Path.GetDirectoryName(assyPath);
      var path = Path.Combine(assyDir, file.FileName);
      await using var fs = System.IO.File.Create(path);
      await using var stream = file.OpenReadStream();
      await stream.CopyToAsync(fs);

      return Ok();
    }
  }
}
