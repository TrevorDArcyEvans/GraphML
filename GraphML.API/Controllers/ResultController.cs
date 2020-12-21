using GraphML.API.Attributes;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
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
    /// Delete results for a completed job
    /// </summary>
    /// <param name="correlationId">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpDelete]
    [Route("Delete/{correlationId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public IActionResult Delete([FromRoute][Required] Guid correlationId)
    {
      _logic.Delete(correlationId);

      return new OkResult();
    }

    /// <summary>
    /// List results for a person
    /// </summary>
    /// <param name="contact">Person</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpGet]
    [Route("List/{contactId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(IEnumerable<IRequest>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public IActionResult List([FromRoute][Required] Contact contact)
    {
      var retval = _logic.List(contact);

      return new OkObjectResult(retval);
    }

    /// <summary>
    /// Get results for a request
    /// </summary>
    /// <param name="correlationId">Job request</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity not found</response>
    [HttpGet]
    [Route("Retrieve/{correlationId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(IResult))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public IActionResult Retrieve([FromRoute][Required] Guid correlationId)
    {
      var retval = _logic.Retrieve(correlationId);

      return new OkObjectResult(retval);
    }
  }
}
