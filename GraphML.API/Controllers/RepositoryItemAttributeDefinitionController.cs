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
  /// Manage RepositoryItemAttributeDefinition
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class RepositoryItemAttributeDefinitionController : OwnedGraphMLController<RepositoryItemAttributeDefinition>
  {
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public RepositoryItemAttributeDefinitionController(IRepositoryItemAttributeDefinitionLogic logic) :
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
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<RepositoryItemAttributeDefinition>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<RepositoryItemAttributeDefinition>> ByIds([FromBody] [Required] IEnumerable<Guid> ids)
    {
      return Ok(ByIdsInternal(ids));
    }

    /// <summary>
    /// Retrieve all Entities in a paged list
    /// </summary>
    /// <param name="ownerIds"></param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success - if no Entities found, return empty list</response>
    [HttpPost]
    [Route(nameof(ByOwners))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<RepositoryItemAttributeDefinition>))]
    public override ActionResult<IEnumerable<RepositoryItemAttributeDefinition>> ByOwners([FromBody] [Required] IEnumerable<Guid> ownerIds, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
    {
      return Ok(ByOwnersInternal(ownerIds, pageIndex - 1, pageSize));
    }

    /// <summary>
    /// Create new Entities
    /// </summary>
    /// <param name="entity">new Entities information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPost]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK, type: typeof(IEnumerable<RepositoryItemAttributeDefinition>))]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<RepositoryItemAttributeDefinition>> Create([FromBody] [Required] IEnumerable<RepositoryItemAttributeDefinition> entity)
    {
      return Ok(CreateInternal(entity));
    }

    /// <summary>
    /// Delete existing Entities
    /// </summary>
    /// <param name="entity">existing Entities information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in </response>
    [HttpDelete]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int) HttpStatusCode.NotFound)]
    public override ActionResult Delete([FromBody] [Required] IEnumerable<RepositoryItemAttributeDefinition> entity)
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
    public override ActionResult Update([FromBody] [Required] IEnumerable<RepositoryItemAttributeDefinition> entity)
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
  }
}
