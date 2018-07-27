using GraphML.Interfaces;
using GraphML.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GraphML.API.Controllers
{
#pragma warning disable CS1591
  public abstract class GraphMLController<T> : ControllerBase
  {
    private readonly ILogic<T> _logic;

    public GraphMLController(ILogic<T> logic)
    {
      _logic = logic;
    }

    public abstract IActionResult ByIds([FromBody]IEnumerable<string> ids);
    protected IActionResult ByIdsInternal([FromBody]IEnumerable<string> ids)
    {
      var ent = _logic.Ids(ids);
      return ent != null ? (IActionResult)new OkObjectResult(ent) : new NotFoundResult();
    }

    public abstract IActionResult ByOwners([FromBody]IEnumerable<string> ownerIds, [FromQuery]int? pageIndex, [FromQuery]int? pageSize);
    protected IActionResult ByOwnersInternal([FromBody]IEnumerable<string> ownerIds, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var result = _logic.ByOwners(ownerIds);
      var retval = PaginatedList<T>.Create(result, pageIndex, pageSize);
      return new OkObjectResult(retval);
    }

    public abstract IActionResult Create([FromBody]IEnumerable<T> entity);
    protected IActionResult CreateInternal([FromBody]IEnumerable<T> entity)
    {
      var newEnt = _logic.Create(entity);
      return newEnt != null ? (IActionResult)new OkObjectResult(newEnt) : new NotFoundResult();
    }

    public abstract IActionResult Update([FromBody]IEnumerable<T> entity);
    protected IActionResult UpdateInternal([FromBody]IEnumerable<T> entity)
    {
      _logic.Update(entity);
      return new OkResult();
    }

    public abstract IActionResult Delete([FromBody]IEnumerable<T> entity);
    protected IActionResult DeleteInternal([FromBody]IEnumerable<T> entity)
    {
      _logic.Delete(entity);
      return new OkResult();
    }
  }
#pragma warning restore CS1591
}
