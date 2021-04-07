using GraphML.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GraphML.API.Controllers
{
  public abstract class GraphMLController<T> : ControllerBase where T : Item
  {
    protected const int DefaultPageIndex = 0;
    protected const int DefaultPageSize = 20;

    protected readonly ILogic<T> _logic;

    public GraphMLController(ILogic<T> logic)
    {
      _logic = logic;
    }

    public abstract ActionResult<IEnumerable<T>> ByIds([FromBody][Required] IEnumerable<Guid> ids);
    protected IEnumerable<T> ByIdsInternal(IEnumerable<Guid> ids)
    {
      var ent = _logic.ByIds(ids);
      return ent ?? Enumerable.Empty<T>();
    }

    public abstract ActionResult<IEnumerable<T>> Create([FromBody][Required] IEnumerable<T> entity);
    protected IEnumerable<T> CreateInternal(IEnumerable<T> entity)
    {
      var newEnt = _logic.Create(entity);
      return newEnt ?? Enumerable.Empty<T>();
    }

    public abstract ActionResult Update([FromBody][Required] IEnumerable<T> entity);
    protected void UpdateInternal(IEnumerable<T> entity)
    {
      _logic.Update(entity);
    }

    public abstract ActionResult Delete([FromBody][Required] IEnumerable<T> entity);
    protected void DeleteInternal(IEnumerable<T> entity)
    {
      _logic.Delete(entity);
    }
  }
}
