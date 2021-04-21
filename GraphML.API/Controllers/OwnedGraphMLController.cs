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

    public abstract ActionResult<IEnumerable<T>> ByOwners([FromBody] [Required] IEnumerable<Guid> ownerIds, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize);
    public abstract ActionResult<IEnumerable<T>> ByOwner([FromRoute] [Required] Guid ownerId, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize);

    protected IEnumerable<T> ByOwnersInternal(IEnumerable<Guid> ownerIds, int pageIndex = DefaultPageIndex, int pageSize = DefaultPageSize)
    {
      var result = _ownedLogic.ByOwners(ownerIds, pageIndex, pageSize);
      return result;
    }

    public abstract ActionResult<int> Count([FromQuery] Guid ownerId);

    public int CountInternal(Guid ownerId)
    {
      var result = _ownedLogic.Count(ownerId);
      return result;
    }
  }
}
