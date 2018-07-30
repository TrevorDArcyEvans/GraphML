﻿using GraphML.API.Attributes;
using GraphML.Interfaces.Porcelain;
using GraphML.Porcelain;
using GraphML.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace GraphML.API.Controllers.Porcelain
{
  /// <summary>
  /// Manage GraphEx
  /// </summary>
  [ApiVersion("1")]
  [ApiTag("porcelain")]
  [Route("api/porcelain/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.User + "," + Roles.UserAdmin,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class GraphExController : GraphMLController<GraphEx>
  {
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public GraphExController(IGraphExLogic logic) :
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
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<GraphEx>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Entity with identifier not found")]
    public override IActionResult ByIds([FromBody] IEnumerable<string> ids)
    {
      return ByIdsInternal(ids);
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
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<GraphEx>), description: "Success - if no Entities found, return empty list")]
    public override IActionResult ByOwners([FromBody] IEnumerable<string> ownerIds, [FromQuery] int? pageIndex, [FromQuery] int? pageSize)
    {
      return ByOwnersInternal(ownerIds, pageIndex, pageSize);
    }

    /// <summary>
    /// Create new Entities
    /// </summary>
    /// <param name="entity">new Entities information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPost]
    [Route(nameof(Create))]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<GraphEx>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entities")]
    public override IActionResult Create([FromBody] IEnumerable<GraphEx> entity)
    {
      return CreateInternal(entity);
    }

    /// <summary>
    /// Delete existing Entities
    /// </summary>
    /// <param name="entity">existing Entities information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpDelete]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entities")]
    public override IActionResult Delete([FromBody] IEnumerable<GraphEx> entity)
    {
      return DeleteInternal(entity);
    }

    /// <summary>
    /// Update an existing Entites with new information
    /// </summary>
    /// <param name="entity">Entities with updated information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPut]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entity")]
    public override IActionResult Update([FromBody] IEnumerable<GraphEx> entity)
    {
      return UpdateInternal(entity);
    }
  }
}