using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProcessManagement.Core;
using ProcessManagement.API.ApiModels;
using ProcessManagement.Infrastructure;
using ProcessManagement.Core.Entities;
using ProcessManagement.Core.Services;
using System.Security.Claims;

namespace ProcessManagement.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public ProjectsController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var user = _authService.GetLoggedInUser();

            Specification<Project> specification = new()
            {
                Criteria = (p) => p.CreatedBy == user.Id,
            };

            var list = _unitOfWork.ProjectRepository.Get(specification).ToList();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            Specification<Project> specification = new()
            {
                Criteria = (p) => p.Id == id
            };

            var model = _unitOfWork.ProjectRepository.Get(specification).FirstOrDefault();

            if (model == null) return NotFound();

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProject createProjectRequest)
        {
            var user = _authService.GetLoggedInUser();
            var createdProject = _unitOfWork.ProjectRepository.Add(createProjectRequest.ToProject(user.Id));
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetById", new { id = createdProject.Id }, createdProject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateProject updateProjectRequest)
        {
            var originalProject = _unitOfWork.ProjectRepository.Get(new Specification<Project>
            {
                Criteria = (p) => p.Id == id,
            }).FirstOrDefault();

            originalProject.Name = updateProjectRequest.Name;
            originalProject.Description = updateProjectRequest.Description;

            var updatedProject = _unitOfWork.ProjectRepository.Update(originalProject);
            await _unitOfWork.CompleteAsync();

            return Ok(updatedProject);
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> Delete([FromRoute] string projectId)
        {
            var toDelete = _unitOfWork.ProjectRepository.Get(new Specification<Project>
            {
                Criteria = p => p.Id == projectId
            }).FirstOrDefault();

            var workItemsToRemove = _unitOfWork.WorkItemRepository.Get(new Specification<WorkItem>
            {
                Criteria = w => w.Project.Id == projectId
            });

            _unitOfWork.WorkItemRepository.RemoveRange(workItemsToRemove);
            _unitOfWork.ProjectRepository.Remove(toDelete);

            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
