using GraphML.API.Attributes;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage Node
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class NodeController : RepositoryItemController<Node>
  {
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public NodeController(INodeLogic logic) :
      base(logic)
    {
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<Node>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<Node>> ByIds([FromBody] [Required] IEnumerable<Guid> ids)
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(PagedDataEx<Node>))]
    public override ActionResult<PagedDataEx<Node>> ByOwners(
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(PagedDataEx<Node>))]
    public override ActionResult<PagedDataEx<Node>> ByOwner(
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<Node>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<Node>> Create([FromBody] [Required] IEnumerable<Node> entity)
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
    public override ActionResult Delete([FromBody] [Required] IEnumerable<Node> entity)
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
    public override ActionResult Update([FromBody] [Required] IEnumerable<Node> entity)
    {
      UpdateInternal(entity);
      return Ok();
    }

    /// <summary>
    /// Retrieve parents of specified entity
    /// </summary>
    /// <param name="itemId">child entity</param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity with identifier not found</response>
    [HttpGet]
    [Route(nameof(GetParents) + "/{itemId}")]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(PagedDataEx<Node>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<PagedDataEx<Node>> GetParents([FromRoute] [Required] Guid itemId, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
    {
      return Ok(GetParentsInternal(itemId, pageIndex - 1, pageSize));
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
  }
}
