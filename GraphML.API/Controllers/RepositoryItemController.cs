using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GraphML.API.Controllers
{
  public abstract class RepositoryItemController<T> : OwnedGraphMLController<T> where T : RepositoryItem
  {
    private readonly IRepositoryItemLogic<T> _repoItemLogic;

    public RepositoryItemController(IRepositoryItemLogic<T> logic) :
      base(logic)
    {
      _repoItemLogic = logic;
    }

    public abstract ActionResult<IEnumerable<T>> GetParents([FromBody] [Required] Guid itemId, [FromQuery] int pageIndex = DefaultPageIndex, [FromQuery] int pageSize = DefaultPageSize);

    protected IEnumerable<T> GetParentsInternal(Guid itemId, int pageIndex = DefaultPageIndex, int pageSize = DefaultPageSize)
    {
      var result = _repoItemLogic.GetParents(itemId, pageIndex, pageSize);
      return result;
    }
  }
}
