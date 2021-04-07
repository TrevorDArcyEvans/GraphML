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
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    public override ActionResult<IEnumerable<Role>> ByIds([FromBody][Required] IEnumerable<Guid> ids)
    {
      return Ok(ByIdsInternal(ids));
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
    public override ActionResult<IEnumerable<Role>> Create([FromBody][Required] IEnumerable<Role> entity)
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
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK)]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override ActionResult Delete([FromBody][Required] IEnumerable<Role> entity)
    {
      DeleteInternal(entity);
      return Ok();
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
    public override ActionResult Update([FromBody][Required] IEnumerable<Role> entity)
    {
      UpdateInternal(entity);
      return Ok();
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
		public ActionResult<IEnumerable<Role>> ByContactId([FromRoute][Required] Guid id)
		{
			var retval = _roleLogic.ByContactId(id);

			return Ok(retval);
		}

		/// <summary>
		/// List all roles in the system
		/// </summary>
		/// <param name="pageIndex">0-based index of page to return.  Defaults to 0</param>
		/// <param name="pageSize">number of items per page.  Defaults to 20</param>
		/// <response code="200">Success - if no Entities found, return empty list</response>
		[HttpGet]
		[Route(nameof(GetAll))]
		[ValidateModelState]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(IEnumerable<Role>))]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
		public ActionResult<IEnumerable<Role>> GetAll([FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
		{
			var result = _roleLogic.GetAll()
				.Skip(pageIndex * pageSize)
				.Take(pageSize);
			return Ok(result);
		}

		/// <summary>
		/// List all roles for the calling user
		/// </summary>
		/// <param name="pageIndex">0-based index of page to return.  Defaults to 0</param>
		/// <param name="pageSize">number of items per page.  Defaults to 20</param>
		/// <response code="200">Success - if no Entities found, return empty list</response>
		[HttpGet]
		[Route(nameof(Get))]
		[ValidateModelState]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.OK, Type = typeof(IEnumerable<Role>))]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
		public ActionResult<IEnumerable<Role>> Get([FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
		{
			var result = _roleLogic.Get()
				.Skip(pageIndex * pageSize)
				.Take(pageSize);
			return Ok(result);
		}

    /// <summary>
    /// Retrieve total number of Roles
    /// </summary>
    /// <response code="200">Success - if no Roles found, return zero</response>
    [HttpGet]
    [Route(nameof(Count))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(int))]
    public ActionResult<int> Count()
    {
      var result = _roleLogic.Count();
      return Ok(result);
    }
	}
}