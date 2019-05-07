using GraphML.Interfaces;
using GraphML.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphML.API.Controllers
{
#pragma warning disable CS1591
  public abstract class OwnedGraphMLController<T> : GraphMLController<T>
  {
    private readonly IOwnedLogic<T> _ownedLogic;

    public OwnedGraphMLController(IOwnedLogic<T> logic) :
      base(logic)
    {
      _ownedLogic = logic;
    }

    public abstract IActionResult ByOwners([FromBody][Required] IEnumerable<string> ownerIds, [FromQuery]int pageIndex = DefaultPageIndex, [FromQuery]int pageSize = DefaultPageSize);
    protected IActionResult ByOwnersInternal([FromBody][Required] IEnumerable<string> ownerIds, [FromQuery]int pageIndex = DefaultPageIndex, [FromQuery]int pageSize = DefaultPageSize)
    {
      var result = _ownedLogic.ByOwners(ownerIds, pageIndex, pageSize);
      return new OkObjectResult(result);
    }
  }
#pragma warning restore CS1591
}
