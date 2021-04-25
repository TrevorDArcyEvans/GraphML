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

    /// <summary>
    /// Retrieve Entity by its unique identifier
    /// </summary>
    /// <param name="ids">unique identifier</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity with identifier not found</response>
    [HttpPost]
    [Route(nameof(ByIds))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<ChartEx>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<ChartEx>> ByIds(IEnumerable<Guid> ids)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Create new Entities
    /// </summary>
    /// <param name="entity">new Entities information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPost]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<ChartEx>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<ChartEx>> Create(IEnumerable<ChartEx> entity)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Update an existing Entities with new information
    /// </summary>
    /// <param name="entity">Entities with updated information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPut]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult Update(IEnumerable<ChartEx> entity)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Delete existing Entities
    /// </summary>
    /// <param name="entity">existing Entities information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpDelete]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult Delete(IEnumerable<ChartEx> entity)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieve all Entities in a paged list
    /// </summary>
    /// <param name="ownerIds"></param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <param name="searchTerm">user entered string in search box.  Defaults to null</param>
    /// <response code="200">Success - if no Entities found, return empty list</response>
    [HttpPost]
    [Route(nameof(ByOwners))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(PagedDataEx<ChartEx>))]
    public override ActionResult<PagedDataEx<ChartEx>> ByOwners(
      IEnumerable<Guid> ownerIds, 
      int pageIndex = DefaultPageIndex, 
      int pageSize = DefaultPageSize, 
      string searchTerm = null)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieve Entities in a paged list
    /// </summary>
    /// <param name="ownerId">identifier of owner</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <param name="searchTerm">user entered string in search box.  Defaults to null</param>
    /// <response code="200">Success - if no Entities found, return empty list</response>
    [HttpGet]
    [Route(nameof(ByOwner) + "/{ownerId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(PagedDataEx<ChartEx>))]
    public override ActionResult<PagedDataEx<ChartEx>> ByOwner(
      Guid ownerId, 
      int pageIndex = DefaultPageIndex, 
      int pageSize = DefaultPageSize, 
      string searchTerm = null)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieve total number of Entities
    /// </summary>
    /// <param name="ownerId">identifier of owner</param>
    /// <response code="200">Success - if no Entities found, return zero</response>
    [HttpGet]
    [Route(nameof(Count) + "/{ownerId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(int))]
    public override ActionResult<int> Count(Guid ownerId)
    {
      throw new NotImplementedException();
    }
  }
}
