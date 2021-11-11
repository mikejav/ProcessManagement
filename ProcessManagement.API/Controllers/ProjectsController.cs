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

namespace ProcessManagement.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            Specification<Project> specification = new();

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
            var createdProject = _unitOfWork.ProjectRepository.Add(createProjectRequest.ToProject());
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetById", new { id = createdProject.Id }, createdProject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateProject updateProjectRequest)
        {
            var updatedProject = _unitOfWork.ProjectRepository.Update(updateProjectRequest.ToProject(id));
            await _unitOfWork.CompleteAsync();

            return Ok(updatedProject);
        }
    }
}
