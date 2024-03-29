﻿using System;
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
  /// Manage GraphNode
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class GraphNodeController : OwnedGraphMLController<GraphNode>
  {
    private readonly IGraphNodeLogic _graphNodeLogic;
    
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public GraphNodeController(IGraphNodeLogic logic) :
      base(logic)
    {
      _graphNodeLogic = logic;
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<GraphNode>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<GraphNode>> ByIds([FromBody] [Required] IEnumerable<Guid> ids)
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(PagedDataEx<GraphNode>))]
    public override ActionResult<PagedDataEx<GraphNode>> ByOwners(
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(PagedDataEx<GraphNode>))]
    public override ActionResult<PagedDataEx<GraphNode>> ByOwner(
      [FromRoute] [Required] Guid ownerId,
      [FromQuery] int pageIndex = DefaultPageIndex,
      [FromQuery] int pageSize = DefaultPageSize,
      [FromQuery] string searchTerm = null)
    {
      return Ok(ByOwnersInternal(new[] { ownerId }, pageIndex- 1, pageSize, searchTerm));
    }

    /// <summary>
    /// Create new Entities
    /// </summary>
    /// <param name="entity">new Entities information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPost]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<GraphNode>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<GraphNode>> Create([FromBody] [Required] IEnumerable<GraphNode> entity)
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
    public override ActionResult Delete([FromBody] [Required] IEnumerable<GraphNode> entity)
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
    public override ActionResult Update([FromBody] [Required] IEnumerable<GraphNode> entity)
    {
      UpdateInternal(entity);
      return Ok();
    }

    /// <summary>
    /// Add new Entities to a Graph by specifying a filter on underlying Repository
    /// </summary>
    /// <param name="graphId">unique identifier of Graph</param>
    /// <param name="filter">Repository filter for new Entities</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPost]
    [Route(nameof(AddByFilter) + "/{graphId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<GraphNode>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<GraphNode>> AddByFilter([FromRoute] [Required] Guid graphId, [FromBody] string filter)
    {
      var retval = _graphNodeLogic.AddByFilter(graphId, filter);
      return Ok(retval);
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
    /// Retrieve a <see cref="GraphNode"/> corresponding to an underlying <see cref="Item"/>
    /// from a specified <see cref="Graph"/>, if it exists
    /// </summary>
    /// <param name="graphId">identifier of <see cref="Graph"/></param>
    /// <param name="repoItemIds">identifier of underlying <see cref="Item"/></param>
    /// <response code="200">Success - if no Entities found, returns null</response>
    [HttpPost]
    [Route(nameof(ByRepositoryItems) + "/{graphId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<GraphNode>))]
    public ActionResult<IEnumerable<GraphNode>> ByRepositoryItems(
      [FromRoute] Guid graphId,
      [FromBody] IEnumerable<Guid> repoItemIds)
    {
      return Ok(_graphNodeLogic.ByRepositoryItems(graphId, repoItemIds));
    }
  }
}
