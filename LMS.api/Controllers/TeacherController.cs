﻿using LMS.Application.Interfaces;
using LMS.Data.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeacherController(ITeacherService teacherService) : ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;

        [HttpGet("count")]
        public async Task<IActionResult> GetTeachersCount()
        {
            return Ok(await _teacherService.GetTeachersCount());
        }

        [Authorize(Roles = ConstRoles.Teacher)]
        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage(IFormFile newImage)
        {
            var result = await _teacherService.EditTeacherImage(newImage);
            return result ? Ok("updated successfully") : BadRequest("failed to update");

        }


        [Authorize(Roles = ConstRoles.Teacher)]
        [HttpDelete("image")]
        public async Task<IActionResult> DeleteImage()
        {
            var result = await _teacherService.DeleteTeacherPictureAsync();
            return result ? Ok("deleted successfully") : BadRequest("failed to delete");

        }
    }
}
