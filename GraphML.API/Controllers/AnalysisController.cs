using GraphML.Analysis.SNA.Centrality;
using GraphML.API.Attributes;
using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
    private readonly IAnalysisLogic _logic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public AnalysisController(
      IAnalysisLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Calculate SNA 'Degree' for specified graph
    /// </summary>
    /// <param name="req">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpPost]
    [Route(nameof(Degree))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public IActionResult Degree([FromBody][Required] DegreeRequest req)
    {
      _logic.Degree(req);

      return new OkObjectResult(req.CorrelationId);
    }

    /// <summary>
    /// Calculate SNA 'Closeness' for specified graph
    /// </summary>
    /// <param name="req">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpPost]
    [Route(nameof(Closeness))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public IActionResult Closeness([FromBody][Required] ClosenessRequest req)
    {
      _logic.Closeness(req);

      return new OkObjectResult(req.CorrelationId);
    }

    /// <summary>
    /// Calculate SNA 'Closeness' for specified graph
    /// </summary>
    /// <param name="req">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpPost]
    [Route(nameof(Betweenness))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public IActionResult Betweenness([FromBody][Required] BetweennessRequest req)
    {
      _logic.Betweenness(req);

      return new OkObjectResult(req.CorrelationId);
    }
  }
}
