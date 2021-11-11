using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcessManagement.API.ApiModels;
using ProcessManagement.Core;
using ProcessManagement.Core.Entities;
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

        public WorkItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] string projectId)
        {
            Specification<WorkItem> specification = new Specification<WorkItem>
            {
                Criteria = (w) => w.Project.Id == projectId,
            };

            var list = _unitOfWork.WorkItemRepository.Get(specification);

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
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
        public async Task<IActionResult> Create([FromBody] CreateWorkItem createWorkItemRequest)
        {
            var createdWorkItem = _unitOfWork.WorkItemRepository.Add(createWorkItemRequest.ToWorkItem());
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetById", new { id = createdWorkItem.Id }, createdWorkItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateWorkItem updateWorkItemRequest)
        {
            var updatedProject = _unitOfWork.WorkItemRepository.Update(updateWorkItemRequest.ToWorkItem(id));
            await _unitOfWork.CompleteAsync();

            return Ok(updatedProject);
        }
    }
}
