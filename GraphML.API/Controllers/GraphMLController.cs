﻿using GraphML.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphML.API.Controllers
{
  public abstract class GraphMLController<T> : ControllerBase where T : Item
  {
    protected const int DefaultPageIndex = 1;
    protected const int DefaultPageSize = 20;

    protected readonly ILogic<T> _logic;

    public GraphMLController(ILogic<T> logic)
    {
      _logic = logic;
    }

    public abstract IActionResult ByIds([FromBody][Required] IEnumerable<Guid> ids);
    protected IActionResult ByIdsInternal([FromBody][Required] IEnumerable<Guid> ids)
    {
      var ent = _logic.ByIds(ids);
      return ent != null ? (IActionResult)new OkObjectResult(ent) : new NotFoundResult();
    }

    public abstract IActionResult Create([FromBody][Required] IEnumerable<T> entity);
    protected IActionResult CreateInternal([FromBody][Required] IEnumerable<T> entity)
    {
      var newEnt = _logic.Create(entity);
      return newEnt != null ? (IActionResult)new OkObjectResult(newEnt) : new NotFoundResult();
    }

    public abstract IActionResult Update([FromBody][Required] IEnumerable<T> entity);
    protected IActionResult UpdateInternal([FromBody][Required] IEnumerable<T> entity)
    {
      _logic.Update(entity);
      return new OkResult();
    }

    public abstract IActionResult Delete([FromBody][Required] IEnumerable<T> entity);
    protected IActionResult DeleteInternal([FromBody][Required] IEnumerable<T> entity)
    {
      _logic.Delete(entity);
      return new OkResult();
    }
  }
}
