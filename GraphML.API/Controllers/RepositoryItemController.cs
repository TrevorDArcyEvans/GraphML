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

        public abstract IActionResult GetParents([FromBody][Required] T entity, [FromQuery]int pageIndex = DefaultPageIndex, [FromQuery]int pageSize = DefaultPageSize);
        protected IActionResult GetParentsInternal([FromBody][Required] T entity, [FromQuery]int pageIndex = DefaultPageIndex, [FromQuery]int pageSize = DefaultPageSize)
        {
            var result = _repoItemLogic.GetParents(entity, pageIndex, pageSize);
            return new OkObjectResult(result);
        }
    }
}