using GraphML.API.Attributes;
using GraphML.Interfaces;
using GraphML.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage Organisation
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.User + "," + Roles.UserAdmin,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class OrganisationController : GraphMLController<Organisation>
  {
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public OrganisationController(IOrganisationLogic logic) :
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
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Organisation>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Entity with identifier not found")]
    public override IActionResult ByIds([FromBody] IEnumerable<string> ids)
    {
      return ByIdsInternal(ids);
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
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Organisation>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entities")]
    public override IActionResult Create([FromBody] IEnumerable<Organisation> entity)
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
    public override IActionResult Delete([FromBody] IEnumerable<Organisation> entity)
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
    public override IActionResult Update([FromBody] IEnumerable<Organisation> entity)
    {
      return UpdateInternal(entity);
    }

    /// <summary>
    /// Retrieve all Entities in a paged list
    /// </summary>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success - if no Entities found, return empty list</response>
    [HttpGet]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<Organisation>), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Entity not found")]
    public IActionResult GetAll([FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var result = ((IOrganisationLogic)_logic).GetAll();
      var retval = PaginatedList<Organisation>.Create(result, pageIndex, pageSize);
      return new OkObjectResult(retval);
    }
  }
}
