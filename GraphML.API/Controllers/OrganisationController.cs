using GraphML.API.Attributes;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GraphML.API.Controllers
{
  /// <summary>
  /// Manage Organisation
  /// </summary>
  [ApiVersion("1")]
    [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Produces("application/json")]
  public sealed class OrganisationController : GraphMLController<Organisation>
  {
    private readonly IOrganisationLogic _orgLogic;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="logic">business logic</param>
    public OrganisationController(IOrganisationLogic logic) :
      base(logic)
    {
      _orgLogic = logic;
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
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Organisation>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<Organisation>> ByIds([FromBody][Required] IEnumerable<Guid> ids)
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
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Organisation>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public override ActionResult<IEnumerable<Organisation>> Create([FromBody][Required] IEnumerable<Organisation> entity)
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
    public override ActionResult Delete([FromBody][Required] IEnumerable<Organisation> entity)
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
    public override ActionResult Update([FromBody][Required] IEnumerable<Organisation> entity)
    {
      UpdateInternal(entity);
      return Ok();
    }

    /// <summary>
    /// Retrieve all Entities in a paged list
    /// </summary>
    /// <param name="pageIndex">0-based index of page to return.  Defaults to 0</param>
    /// <param name="pageSize">number of items per page.  Defaults to 20</param>
    /// <response code="200">Success - if no Entities found, return empty list</response>
    [HttpGet]
    [Route(nameof(GetAll))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(IEnumerable<Organisation>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<Organisation>> GetAll([FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
    {
      var result = _orgLogic.GetAll()
        .Skip(pageIndex * pageSize)
        .Take(pageSize);
      return Ok(result);
    }

    /// <summary>
    /// Retrieve total number of Entities
    /// </summary>
    /// <response code="200">Success - if no Entities found, return zero</response>
    [HttpGet]
    [Route(nameof(Count))]
    [ValidateModelState]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(int))]
    public ActionResult<int> Count()
    {
      var result = _orgLogic.Count();
      return Ok(result);
    }
  }
}
