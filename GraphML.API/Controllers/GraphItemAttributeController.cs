using GraphML.API.Attributes;
using GraphML.Interfaces;
using GraphML.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage GraphItemAttribute
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.User + "," + Roles.UserAdmin,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class GraphItemAttributeController : GraphMLController<GraphItemAttribute>
  {
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public GraphItemAttributeController(IGraphItemAttributeLogic logic) :
      base(logic)
    {
    }

    /// <summary>
    /// Retrieve GraphItemAttribute by its unique identifier
    /// </summary>
    /// <param name="id">unique identifier</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity with identifier not found</response>
    [HttpGet]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(GraphItemAttribute), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Entity with identifier not found")]
    public override IActionResult ById([FromQuery]string id)
    {
      return ByIdInternal(id);
    }

    /// <summary>
    /// Retrieve all GraphItemAttribute in a paged list
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success - if no Entity found, return empty list</response>
    [HttpGet]
    [Route(nameof(ByOwner))]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<GraphItemAttribute>), description: "Success - if no Entity found, return empty list")]
    public override IActionResult ByOwner([FromQuery]string ownerId, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      return ByOwnerInternal(ownerId, pageIndex, pageSize);
    }

    /// <summary>
    /// Create a new GraphItemAttribute
    /// </summary>
    /// <param name="entity">new GraphItemAttribute information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entity</response>
    [HttpPost]
    [Route(nameof(Create))]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(GraphItemAttribute), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entity")]
    public override IActionResult Create([FromBody] GraphItemAttribute entity)
    {
      return CreateInternal(entity);
    }

    /// <summary>
    /// Delete an existing GraphItemAttribute
    /// </summary>
    /// <param name="entity">existing GraphItemAttribute information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entity</response>
    [HttpDelete]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entity")]
    public override IActionResult Delete([FromBody] GraphItemAttribute entity)
    {
      return DeleteInternal(entity);
    }
    /// <summary>
    /// Update an existing GraphItemAttribute with new information
    /// </summary>
    /// <param name="entity">Entity with updated information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entity</response>
    [HttpPut]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entity")]
    public override IActionResult Update([FromBody] GraphItemAttribute entity)
    {
      return UpdateInternal(entity);
    }
  }
}
