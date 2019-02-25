using GraphML.Interfaces;
using GraphML.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GraphML.API.Controllers
{
#pragma warning disable CS1591
  public abstract class OwnedGraphMLController<T> : GraphMLController<T>
  {
    public OwnedGraphMLController(IOwnedLogic<T> logic) :
      base(logic)
    {
    }

    public abstract IActionResult ByOwners([FromBody]IEnumerable<string> ownerIds, [FromQuery]int pageIndex = DefaultPageIndex, [FromQuery]int pageSize = DefaultPageSize);
    protected IActionResult ByOwnersInternal([FromBody]IEnumerable<string> ownerIds, [FromQuery]int pageIndex = DefaultPageIndex, [FromQuery]int pageSize = DefaultPageSize)
    {
      var result = ((IOwnedLogic<T>)_logic).ByOwners(ownerIds, pageIndex, pageSize);
      return new OkObjectResult(result);
    }
  }
#pragma warning restore CS1591
}
