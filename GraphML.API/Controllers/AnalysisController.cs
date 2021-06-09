using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;
using GraphML.API.Attributes;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using GraphML.Analysis.FindDuplicates;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage Analysis
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<Guid> Degree([FromBody] [Required] DegreeRequest req)
    {
      _logic.Degree(req);

      return Ok(req.CorrelationId);
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<Guid> Closeness([FromBody] [Required] ClosenessRequest req)
    {
      _logic.Closeness(req);

      return Ok(req.CorrelationId);
    }

    /// <summary>
    /// Calculate SNA 'Betweenness' for specified graph
    /// </summary>
    /// <param name="req">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpPost]
    [Route(nameof(Betweenness))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<Guid> Betweenness([FromBody] [Required] BetweennessRequest req)
    {
      _logic.Betweenness(req);

      return Ok(req.CorrelationId);
    }

    /// <summary>
    /// Calculate shortest paths between root node and goal node
    /// </summary>
    /// <param name="req">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpPost]
    [Route(nameof(FindShortestPaths))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<Guid> FindShortestPaths([FromBody] [Required] FindShortestPathsRequest req)
    {
      _logic.FindShortestPaths(req);

      return Ok(req.CorrelationId);
    }

    /// <summary>
    /// Find duplicate nodes in a graph using double metaphone algorithm
    /// </summary>
    /// <param name="req">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpPost]
    [Route(nameof(FindDuplicates))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<Guid> FindDuplicates([FromBody] [Required] FindDuplicatesRequest req)
    {
      _logic.FindDuplicates(req);

      return Ok(req.CorrelationId);
    }
  }
}
