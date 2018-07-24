using GraphML.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

    public abstract IActionResult ByOwner([FromQuery]string ownerId, [FromQuery]int? pageIndex, [FromQuery]int? pageSize);
    protected IActionResult ByOwnerInternal([FromQuery]string ownerId, [FromQuery]int? pageIndex, [FromQuery]int? pageSize)
    {
      var result = _logic.ByOwner(ownerId);
      var retval = PaginatedList<T>.Create(result, pageIndex, pageSize);
      return new OkObjectResult(retval);
    }

    public abstract IActionResult Create([FromBody]T entity);
    protected IActionResult CreateInternal([FromBody]T entity)
    {
      var newEnt = _logic.Create(entity);
      return newEnt != null ? (IActionResult)new OkObjectResult(newEnt) : new NotFoundResult();
    }

    public abstract IActionResult Update([FromBody]T entity);
    protected IActionResult UpdateInternal([FromBody]T entity)
    {
      _logic.Update(entity);
      return new OkResult();
    }

    public abstract IActionResult Delete([FromBody]T entity);
    protected IActionResult DeleteInternal([FromBody]T entity)
    {
      _logic.Delete(entity);
      return new OkResult();
    }
  }
#pragma warning restore CS1591
}
