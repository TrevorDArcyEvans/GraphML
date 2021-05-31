using System;
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

    public abstract ActionResult<PagedDataEx<T>> GetParents(
      [FromBody] [Required] Guid itemId,
      [FromQuery] int pageIndex = DefaultPageIndex,
      [FromQuery] int pageSize = DefaultPageSize,
      [FromQuery] string searchTerm = null);

    protected PagedDataEx<T> GetParentsInternal(Guid itemId, int pageIndex = DefaultPageIndex, int pageSize = DefaultPageSize, string searchTerm = null)
    {
      if (pageIndex < 0)
      {
        throw new ArgumentOutOfRangeException($"{nameof(pageIndex)} starts at 0");
      }
      
      var result = _repoItemLogic.GetParents(itemId, pageIndex, pageSize, searchTerm);
      return result;
    }
  }
}
