using GraphML.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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

    public abstract ActionResult<PagedDataEx<T>> ByOwners(IEnumerable<Guid> ownerIds, int pageIndex = DefaultPageIndex, int pageSize = DefaultPageSize, string searchTerm = null);
    public abstract ActionResult<PagedDataEx<T>> ByOwner(Guid ownerId, int pageIndex = DefaultPageIndex, int pageSize = DefaultPageSize, string searchTerm = null);

    protected PagedDataEx<T> ByOwnersInternal(IEnumerable<Guid> ownerIds, int pageIndex = DefaultPageIndex, int pageSize = DefaultPageSize, string searchTerm = null)
    {
      if (pageIndex < 0)
      {
        throw new ArgumentOutOfRangeException($"{nameof(pageIndex)} starts at 0");
      }
      
      var result = _ownedLogic.ByOwners(ownerIds, pageIndex, pageSize, searchTerm);
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
