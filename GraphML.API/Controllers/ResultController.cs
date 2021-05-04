using GraphML.API.Attributes;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using GraphML.Analysis;
using GraphML.Analysis.RankedShortestPath;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage Results
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class ResultController : ControllerBase
  {
    private readonly IResultLogic _logic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public ResultController(
      IResultLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Delete results (<see cref="IResult"/>) for a completed job (<see cref="IRequest"/>)
    /// </summary>
    /// <param name="correlationId">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpDelete]
    [Route(nameof(Delete) + "/{correlationId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult Delete([FromRoute] [Required] Guid correlationId)
    {
      _logic.Delete(correlationId);

      return Ok();
    }

    /// <summary>
    /// List results (<see cref="IResult"/>) for a <see cref="Contact"/>
    /// </summary>
    /// <param name="contactId">Person</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpGet]
    [Route(nameof(ByContact) + "/{contactId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, Type = typeof(IEnumerable<RequestBase>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<RequestBase>> ByContact([FromRoute] [Required] Guid contactId)
    {
      var retval = _logic.ByContact(contactId);

      return Ok(retval);
    }

    /// <summary>
    /// List results (<see cref="IResult"/>) for an <see cref="Organisation"/>
    /// </summary>
    /// <param name="orgId">Organisation</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpGet]
    [Route(nameof(ByOrganisation) + "/{orgId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, Type = typeof(IEnumerable<RequestBase>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<RequestBase>> ByOrganisation([FromRoute] [Required] Guid orgId)
    {
      var retval = _logic.ByOrganisation(orgId);

      return Ok(retval);
    }

    /// <summary>
    /// List results (<see cref="IResult"/>) for a <see cref="Graph"/>
    /// </summary>
    /// <param name="graphId">graph</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpGet]
    [Route(nameof(ByGraph) + "/{graphId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, Type = typeof(IEnumerable<RequestBase>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<RequestBase>> ByGraph([FromRoute] [Required] Guid graphId)
    {
      var retval = _logic.ByGraph(graphId);

      return Ok(retval);
    }

    /// <summary>
    /// Get request (<see cref="IResult"/>) for a job request (<see cref="IRequest"/>)
    /// </summary>
    /// <param name="correlationId">job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpGet]
    [Route(nameof(ByCorrelation) + "/{correlationId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, Type = typeof(RequestBase))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<RequestBase> ByCorrelation([FromRoute] [Required] Guid correlationId)
    {
      var retval = _logic.ByCorrelation(correlationId);

      return Ok(retval);
    }

    /// <summary>
    /// Get results (<see cref="IResult"/>) for a job request (<see cref="IRequest"/>)
    /// </summary>
    /// <param name="correlationId">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpGet]
    [Route(nameof(Retrieve) + "/{correlationId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, Type = typeof(ResultBase))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<ResultBase> Retrieve([FromRoute] [Required] Guid correlationId)
    {
      var retval = _logic.Retrieve(correlationId);

      return Ok(retval);
    }
  }
}
