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
using System;

namespace GraphML.API.Controllers
{
	/// <summary>
	/// Manage Edge
	/// </summary>
	[ApiVersion("1")]
	[Route("api/[controller]")]
	[Authorize(
	  Roles = Roles.Admin + "," + Roles.User + "," + Roles.UserAdmin,
	  AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
	[Produces("application/json")]
	public sealed class EdgeController : RepositoryItemController<Edge>
	{
		private readonly IEdgeLogic _edgeLogic;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="logic">business logic</param>
		public EdgeController(IEdgeLogic logic) :
		  base(logic)
		{
			_edgeLogic = logic;
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
		[ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Edge>))]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
		public override IActionResult ByIds([FromBody][Required] IEnumerable<Guid> ids)
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
		[ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Edge>))]
		public override IActionResult ByOwners([FromBody][Required] IEnumerable<Guid> ownerIds, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
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
		[ValidateModelState]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Edge>))]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
		public override IActionResult Create([FromBody][Required] IEnumerable<Edge> entity)
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
		public override IActionResult Delete([FromBody][Required] IEnumerable<Edge> entity)
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
		public override IActionResult Update([FromBody][Required] IEnumerable<Edge> entity)
		{
			return UpdateInternal(entity);
		}

		/// <summary>
		/// Retrieve Edges connected to specified Nodes
		/// </summary>
		/// <param name="ids">unique identifier</param>
		/// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
		/// <param name="pageSize">number of items per page.  Defaults to 20</param>
		/// <response code="200">Success</response>
		/// <response code="404">Entity with identifier not found</response>
		[HttpPost]
		[Route(nameof(ByNodeIds))]
		[ValidateModelState]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Edge>))]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
		public IActionResult ByNodeIds([FromBody][Required] IEnumerable<Guid> ids, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
		{
			var ents = _edgeLogic.ByNodeIds(ids, pageIndex, pageSize);
			return new OkObjectResult(ents);
		}

		/// <summary>
		/// Retrieve parents of specified entity
		/// </summary>
		/// <param name="entity">child entity</param>
		/// <param name="pageIndex">1-based index of page to return.  Defaults to 1</param>
		/// <param name="pageSize">number of items per page.  Defaults to 20</param>
		/// <response code="200">Success</response>
		/// <response code="404">Entity with identifier not found</response>
		[HttpPost]
		[Route(nameof(GetParents))]
		[ValidateModelState]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Edge>))]
		[ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
		public override IActionResult GetParents([FromBody][Required] Edge entity, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
		{
			var ents = GetParentsInternal(entity, pageIndex, pageSize);
			return new OkObjectResult(ents);
		}
	}
}
