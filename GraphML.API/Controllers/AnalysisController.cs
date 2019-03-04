using GraphML.Analysis.SNA.Centrality;
using GraphML.API.Attributes;
using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage Analysis
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.User + "," + Roles.UserAdmin,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class AnalysisController : ControllerBase
  {
    private readonly IRequestMessageSender _sender;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="sender">configuration</param>
    public AnalysisController(
      IRequestMessageSender sender)
    {
      _sender = sender;
    }

    /// <summary>
    /// Calculate SNA 'Degree' for specified graph
    /// </summary>
    /// <param name="graphId">Unique identifier of graph</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpGet]
    [Route(nameof(Degree))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public IActionResult Degree([Required] string graphId)
    {
      // TODO   get DegreeRequest from body
      var req = new DegreeRequest
      {
        CorrelationId = Guid.NewGuid().ToString(),
        ContactId = Guid.NewGuid().ToString(),  // TODO   refactor into Logic + extract ContactId
        Description = $"Requesting Degree analysis on {graphId}",
        CreatedOnUtc = DateTime.UtcNow,
        GraphId = graphId
      };
      var jsonReq = JsonConvert.SerializeObject(req);
      _sender.Send(jsonReq);

      return new OkObjectResult(req.CorrelationId);
    }
  }
}
