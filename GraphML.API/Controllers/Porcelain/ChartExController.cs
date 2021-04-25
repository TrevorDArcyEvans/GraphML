using GraphML.API.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
  public sealed class ChartExController : OwnedGraphMLController<ChartEx>
  {
    private readonly IChartExLogic _logic;

    public ChartExController(IChartExLogic logic) :
      base(logic)
    {
      _logic = logic;
    }

    /// <summary>
    /// Retrieve <see cref="ChartEx"/> by its <see cref="Chart"/> unique identifier
    /// </summary>
    /// <param name="id">unique identifier of Chart</param>
    /// <response code="200">Success</response>
    /// <response code="404">Chart with identifier not found</response>
    [HttpPost]
    [Route(nameof(ById) + "/{id}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(ChartEx))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<ChartEx> ById([FromRoute] [Required] Guid id)
    {
      return Ok(_logic.ById(id));
    }

    public override ActionResult<IEnumerable<ChartEx>> ByIds(IEnumerable<Guid> ids)
    {
      throw new NotImplementedException();
    }

    public override ActionResult<IEnumerable<ChartEx>> Create(IEnumerable<ChartEx> entity)
    {
      throw new NotImplementedException();
    }

    public override ActionResult Update(IEnumerable<ChartEx> entity)
    {
      throw new NotImplementedException();
    }

    public override ActionResult Delete(IEnumerable<ChartEx> entity)
    {
      throw new NotImplementedException();
    }

    public override ActionResult<PagedDataEx<ChartEx>> ByOwners(
      IEnumerable<Guid> ownerIds, 
      int pageIndex = DefaultPageIndex, 
      int pageSize = DefaultPageSize, 
      string searchTerm = null)
    {
      throw new NotImplementedException();
    }

    public override ActionResult<PagedDataEx<ChartEx>> ByOwner(
      Guid ownerId, 
      int pageIndex = DefaultPageIndex, 
      int pageSize = DefaultPageSize, 
      string searchTerm = null)
    {
      throw new NotImplementedException();
    }

    public override ActionResult<int> Count(Guid ownerId)
    {
      throw new NotImplementedException();
    }
  }
}
