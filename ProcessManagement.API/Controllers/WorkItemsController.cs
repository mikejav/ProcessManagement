using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcessManagement.API.ApiModels;
using ProcessManagement.Core;
using ProcessManagement.Core.Entities;
using ProcessManagement.Core.Services;
using ProcessManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessManagement.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class WorkItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public WorkItemsController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<WorkItem>> GetList([FromQuery] string projectId)
        {
            var user = _authService.GetLoggedInUser();

            Specification<WorkItem> specification = new Specification<WorkItem>
            {
                Criteria = (w) => w.Project.Id == projectId && w.CreatedBy == user.Id,
            };

            var list = _unitOfWork.WorkItemRepository.Get(specification);

            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<WorkItem> GetById([FromRoute] string id)
        {
            Specification<WorkItem> specification = new()
            {
                Criteria = (w) => w.Id == id,
                Includes = new List<System.Linq.Expressions.Expression<Func<WorkItem, object>>>
                {
                    (w) => w.Project,
                }
            };

            var model = _unitOfWork.WorkItemRepository.Get(specification).FirstOrDefault();

            if (model == null) return NotFound();

            return Ok(GetDescribedWorkItem.FromWorkItem(model));
        }

        [HttpPost]
        public async Task<ActionResult<WorkItem>> Create([FromBody] CreateWorkItem createWorkItemRequest)
        {
            var user = _authService.GetLoggedInUser();
            var createdWorkItem = _unitOfWork.WorkItemRepository.Add(createWorkItemRequest.ToWorkItem(user.Id));
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetById", new { id = createdWorkItem.Id }, createdWorkItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkItem>> Update([FromRoute] string id, [FromBody] UpdateWorkItem updateWorkItemRequest)
        {
            var user = _authService.GetLoggedInUser();
            var updatedWorkItem = _unitOfWork.WorkItemRepository.Update(updateWorkItemRequest.ToWorkItem(id, user.Id));
            await _unitOfWork.CompleteAsync();

            return Ok(updatedWorkItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            _unitOfWork.WorkItemRepository.Remove(new WorkItem {
                Id = id,
            });
            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
