using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using GraphML.API.Attributes;
using GraphML.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    [Route(nameof(GetAPIUserClaims))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, Type = typeof(LookupEx<string, string>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<LookupEx<string, string>> GetAPIUserClaims()
    {
      var retval = new LookupEx<string, string>();
      var claims = User.Claims;
      foreach (var claim in claims)
      {
        var grping = retval.GetGrouping(claim.Type, true);
        grping.Add(claim.Value);
      }

      return Ok(retval);
    }
  }
}
