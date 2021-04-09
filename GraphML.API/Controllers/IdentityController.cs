using System.Linq;
using System.Net;
using GraphML.API.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// List claims for current user
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class IdentityController : ControllerBase
  {
    /// <summary>
    /// List claims for current user
    /// </summary>
    /// <response code="200">Success - if no Entities found, return empty list</response>
    [HttpGet]
    [Route(nameof(GetAPIUserClaimsJson))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, Type = typeof(JsonResult))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<JsonResult> GetAPIUserClaimsJson()
    {
      return Ok(new JsonResult(from c in User.Claims select new {c.Type, c.Value}));
    }
  }
}
