using LMS.Application.DTOs;
using LMS.Application.Interfaces;
using LMS.Data.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LiveClassController(ILiveClassService liveClassService) : ControllerBase
    {
        private readonly ILiveClassService _liveClassService = liveClassService;

        [Authorize(Roles = ConstRoles.Teacher)]
        [HttpPost]
        public async Task<IActionResult> CreateLiveClass([FromForm] AddLiveClassDTO liveClassDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _liveClassService.CreateLiveClass(liveClassDto);
            return result ? Ok("created successfully") : BadRequest("failed to create");
        }


        [Authorize(Roles = ConstRoles.Teacher)]
        [HttpPut]
        public async Task<IActionResult> UpdateLiveClass(string id, EditLiveClassDTO liveClassDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _liveClassService.UpdateLiveClass(id, liveClassDto);
            return result ? Ok("updated successfully") : BadRequest("failed to update");
        }


        [Authorize(Roles = ConstRoles.Teacher)]
        [HttpDelete]
        public async Task<IActionResult> DeleteLiveClass(string id)
        {
            var result = await _liveClassService.DeleteLiveClass(id);
            return result ? Ok("deleted successfully") : BadRequest("failed to delete");
        }

        [Authorize(Roles = ConstRoles.Teacher)]
        [HttpGet("teacher-live-Classes")]
        public async Task<IActionResult> GetCurrentTeacherLiveClasses()
        {
            return Ok(await _liveClassService.GetCurrentTeacherLiveClasses());
        }

        [Authorize(Roles = ConstRoles.Student)]
        [HttpGet("student-live-Classes")]
        public async Task<IActionResult> GeStudentLiveClasses()
        {
            return Ok(await _liveClassService.GeStudentLiveClasses());
        }
    }
}
