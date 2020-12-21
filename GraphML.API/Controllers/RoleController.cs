using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using GraphML.API.Attributes;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraphML.API.Controllers
{
	/// <summary>
	/// Manage Roles
	/// </summary>
	[ApiVersion("1")]
	[Route("api/[controller]")]
	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Produces("application/json")]
	public sealed class RoleController : GraphMLController<Role>
	{
		private readonly IRoleLogic _roleLogic;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="logic">business logic</param>
		public RoleController(IRoleLogic logic) :
	      base(logic)
		{
        _roleLogic = logic;
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
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Role>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override IActionResult ByIds([FromBody][Required] IEnumerable<Guid> ids)
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
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Role>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override IActionResult Create([FromBody][Required] IEnumerable<Role> entity)
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
    public override IActionResult Delete([FromBody][Required] IEnumerable<Role> entity)
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
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override IActionResult Update([FromBody][Required] IEnumerable<Role> entity)
    {
      return UpdateInternal(entity);
    }

		/// <summary>
		/// List roles for a person
		/// </summary>
		/// <param name="id">Person</param>
		/// <response code="200">Success</response>
		/// <response code="404">Entity not found</response>
		[HttpGet]
		[Route(nameof(ByContactId) + "/{id}")]
		[ValidateModelState]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(IEnumerable<Role>))]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
		public IActionResult ByContactId([FromRoute][Required] string id)
		{
			var retval = _roleLogic.ByContactId(id);

			return new OkObjectResult(retval);
		}

		/// <summary>
		/// List all roles in the system
		/// </summary>
		/// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
		/// <param name="pageSize">number of items per page.  Defaults to 20</param>
		/// <response code="200">Success - if no Entities found, return empty list</response>
		[HttpGet]
		[Route(nameof(GetAll))]
		[ValidateModelState]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(IEnumerable<Role>))]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
		public IActionResult GetAll([FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
		{
			var result = _roleLogic.GetAll()
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize);
			return new OkObjectResult(result);
		}
	}
}