using GraphML.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphML.API.Controllers
{
  public abstract class OwnedGraphMLController<T> : GraphMLController<T> where T : OwnedItem
  {
    private readonly IOwnedLogic<T> _ownedLogic;

    public OwnedGraphMLController(IOwnedLogic<T> logic) :
      base(logic)
    {
      _ownedLogic = logic;
    }

    public abstract IActionResult ByOwners([FromBody][Required] IEnumerable<Guid> ownerIds, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize);
    protected IActionResult ByOwnersInternal([FromBody][Required] IEnumerable<Guid> ownerIds, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize)
    {
      var result = _ownedLogic.ByOwners(ownerIds, pageIndex, pageSize);
      return new OkObjectResult(result);
    }

    public abstract IActionResult Count([FromQuery] Guid ownerId);
    public IActionResult CountInternal([FromQuery] Guid ownerId)
    {
      var result = _ownedLogic.Count(ownerId);
      return new OkObjectResult(result);
    }
  }
}
