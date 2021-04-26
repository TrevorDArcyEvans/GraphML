using GraphML.API.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using GraphML.Interfaces.Porcelain;
using GraphML.Porcelain;

namespace GraphML.API.Controllers.Porcelain
{
  /// <summary>
  /// Retrieve <see cref="ChartEx"/>
  /// </summary>
  [ApiVersion("1")]
  [Route("api/porcelain/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class ChartExController : ControllerBase
  {
    private readonly IChartExLogic _logic;

    public ChartExController(IChartExLogic logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Retrieve <see cref="ChartEx"/> by its <see cref="Chart"/> unique identifier
    /// </summary>
    /// <param name="id">unique identifier of Chart</param>
    /// <response code="200">Success</response>
    /// <response code="404">Chart with identifier not found</response>
    [HttpGet]
    [Route(nameof(ById) + "/{id}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(ChartEx))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<ChartEx> ById([FromRoute] [Required] Guid id)
    {
      return Ok(_logic.ById(id));
    }
  }
}
