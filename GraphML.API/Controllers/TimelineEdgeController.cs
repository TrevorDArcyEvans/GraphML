using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using GraphML.API.Attributes;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage TimelineEdge
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class TimelineEdgeController : OwnedGraphMLController<TimelineEdge>
  {
    private readonly ITimelineEdgeLogic _timelineEdgeLogic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public TimelineEdgeController(ITimelineEdgeLogic logic) :
      base(logic)
    {
      _timelineEdgeLogic = logic;
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<TimelineEdge>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<TimelineEdge>> ByIds([FromBody] [Required] IEnumerable<Guid> ids)
    {
      return Ok(ByIdsInternal(ids));
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(PagedDataEx<TimelineEdge>))]
    public override ActionResult<PagedDataEx<TimelineEdge>> ByOwners(
      [FromBody] [Required] IEnumerable<Guid> ownerIds,
      [FromQuery] int pageIndex = DefaultPageIndex,
      [FromQuery] int pageSize = DefaultPageSize,
      [FromQuery] string searchTerm = null)
    {
      return Ok(ByOwnersInternal(ownerIds, pageIndex - 1, pageSize, searchTerm));
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(PagedDataEx<TimelineEdge>))]
    public override ActionResult<PagedDataEx<TimelineEdge>> ByOwner(
      [FromRoute] [Required] Guid ownerId,
      [FromQuery] int pageIndex = DefaultPageIndex,
      [FromQuery] int pageSize = DefaultPageSize,
      [FromQuery] string searchTerm = null)
    {
      return Ok(ByOwnersInternal(new[] { ownerId }, pageIndex - 1, pageSize, searchTerm));
    }

    /// <summary>
    /// Create new Entities
    /// </summary>
    /// <param name="entity">new Entities information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPost]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<TimelineEdge>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<TimelineEdge>> Create([FromBody] [Required] IEnumerable<TimelineEdge> entity)
    {
      return Ok(CreateInternal(entity));
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
    public override ActionResult Delete([FromBody] [Required] IEnumerable<TimelineEdge> entity)
    {
      DeleteInternal(entity);
      return Ok();
    }

    /// <summary>
    /// Update existing Entities with new information
    /// </summary>
    /// <param name="entity">Entities with updated information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPut]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult Update([FromBody] [Required] IEnumerable<TimelineEdge> entity)
    {
      UpdateInternal(entity);
      return Ok();
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
    public override ActionResult<int> Count([FromRoute] Guid ownerId)
    {
      return Ok(CountInternal(ownerId));
    }

    /// <summary>
    /// Retrieve a <see cref="TimelineEdge"/> corresponding to an underlying <see cref="GraphItem"/>
    /// from a specified <see cref="Timeline"/>, if it exists
    /// </summary>
    /// <param name="timelineId">identifier of <see cref="Timeline"/></param>
    /// <param name="graphItemId">identifier of underlying <see cref="GraphItem"/></param>
    /// <response code="200">Success - if no Entities found, returns null</response>
    [HttpPost]
    [Route(nameof(ByGraphItems) + "/{timelineId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<TimelineEdge>))]
    public ActionResult<IEnumerable<TimelineEdge>> ByGraphItems(
      [FromRoute] Guid timelineId,
      [FromBody] IEnumerable<Guid> graphItemId)
    {
      return Ok(_timelineEdgeLogic.ByGraphItems(timelineId, graphItemId));
    }
  }
}
