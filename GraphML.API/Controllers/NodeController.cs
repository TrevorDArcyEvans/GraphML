using GraphML.API.Attributes;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using ZNetCS.AspNetCore.Authentication.Basic;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage Node
  /// </summary>
  [ApiVersion("1")]
  [Route("api/[controller]")]
  [Authorize(
    Roles = Roles.Admin + "," + Roles.User + "," + Roles.UserAdmin,
    AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class NodeController : GraphMLController<Node>
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
    /// Retrieve Node by its unique identifier
    /// </summary>
    /// <param name="id">unique identifier</param>
    /// <response code="200">Success</response>
    /// <response code="404">Entity with identifier not found</response>
    [HttpGet]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(Node), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Entity with identifier not found")]
    public override IActionResult ById([FromQuery]string id)
    {
      return ByIdInternal(id);
    }

    /// <summary>
    /// Retrieve all Node in a paged list
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success - if no Node found, return empty list</response>
    [HttpGet]
    [Route(nameof(ByOwner))]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(PaginatedList<Node>), description: "Success - if no Node found, return empty list")]
    public override IActionResult ByOwner([FromQuery]string ownerId, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      return ByOwnerInternal(ownerId, pageIndex, pageSize);
    }

    /// <summary>
    /// Create a new Node
    /// </summary>
    /// <param name="entity">new Node information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entity</response>
    [HttpPost]
    [Route(nameof(Create))]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(Node), description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entity")]
    public override IActionResult Create([FromBody] Node entity)
    {
      return CreateInternal(entity);
    }

    /// <summary>
    /// Delete an existing Node
    /// </summary>
    /// <param name="entity">existing Node information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entity</response>
    [HttpDelete]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entity")]
    public override IActionResult Delete([FromBody] Node entity)
    {
      return DeleteInternal(entity);
    }
    /// <summary>
    /// Update an existing Node with new information
    /// </summary>
    /// <param name="entity">capability with updated information</param>
    /// <response code="200">Success</response>
    /// <response code="404">Incorrect reference in Entity</response>
    [HttpPut]
    [ValidateModelState]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Success")]
    [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, description: "Incorrect reference in Entity")]
    public override IActionResult Update([FromBody] Node entity)
    {
      return UpdateInternal(entity);
    }
  }
}
