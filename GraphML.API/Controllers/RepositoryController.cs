using GraphML.API.Attributes;
using GraphML.Interfaces;
using GraphML.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage RepositoryManager
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.User + "," + Roles.UserAdmin,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class RepositoryController : OwnedGraphMLController<Repository>
  {
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public RepositoryController(IRepositoryLogic logic) :
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
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Repository>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override IActionResult ByIds([FromBody][Required] IEnumerable<string> ids)
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
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Repository>))]
    public override IActionResult ByOwners([FromBody][Required] IEnumerable<string> ownerIds, [FromQuery]int pageIndex = DefaultPageIndex, [FromQuery]int pageSize = DefaultPageSize)
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
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Repository>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override IActionResult Create([FromBody][Required] IEnumerable<Repository> entity)
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
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override IActionResult Delete([FromBody][Required] IEnumerable<Repository> entity)
    {
      return DeleteInternal(entity);
    }
    /// <summary>
    /// Update  existing Entities with new information
    /// </summary>
    /// <param name="entity"> with updated information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entities</response>
    [HttpPut]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override IActionResult Update([FromBody][Required] IEnumerable<Repository> entity)
    {
      return UpdateInternal(entity);
    }
  }
}
