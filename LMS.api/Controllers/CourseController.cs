﻿using LMS.Application.DTOs;
using LMS.Application.Interfaces;
using LMS.Data.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CourseController(ICourseService courseService) : ControllerBase
    {
        private readonly ICourseService _courseService = courseService;

        [Authorize(Roles = ConstRoles.Teacher)]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromForm] CourseDTO courseDto, IFormFile CourseImage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _courseService.CreateCourse(courseDto, CourseImage);
            return result ? Ok("created successfully") : BadRequest("failed to create");
        }


        [Authorize(Roles = ConstRoles.Teacher)]
        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromForm] EditCourseDTO courseDto, IFormFile? CourseImage)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _courseService.UpdateCourse(courseDto.Id, courseDto, CourseImage);
            return result ? Ok("updated successfully") : BadRequest("failed to update");
        }


        [Authorize(Roles = ConstRoles.Teacher)]
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            var result = await _courseService.DeleteCourse(id);
            return result ? Ok("deleted successfully") : BadRequest("failed to delete");
        }


        [Authorize(Roles = ConstRoles.Admin)]
        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudentInCourse(string studentEmail, string courseCode)
        {
            var result = await _courseService.EnrollingStudentInCourse(studentEmail, courseCode);
            return result ? Ok("student has been added successfully") : BadRequest("failed to add student");
        }


        [Authorize]
        [HttpGet("all-academic")]
        public async Task<IActionResult> GetAllAcademicCourses()
        {
            return Ok(await _courseService.GetAcademicCourses());
        }
        [Authorize]
        [HttpGet("all-non-academic")]
        public async Task<IActionResult> GetAllNonAcademicCourses()
        {
            return Ok(await _courseService.GetNonAcademicCourses());
        }
        [Authorize]
        [HttpGet("top-academic")]
        public async Task<IActionResult> GetTopAcademicCourses(int take)
        {
            return Ok(await _courseService.GetTopAcademicCourses(take));
        }
        [Authorize]
        [HttpGet("top-non-academic")]
        public async Task<IActionResult> GetTopNonAcademicCourses(int take)
        {
            return Ok(await _courseService.GetTopNonAcademicCourses(take));
        }
        [Authorize]
        [HttpGet("filter")]
        public async Task<IActionResult> GetCoursesByCrateria(int pageSize, int pageindex, string? subject = "", string? semester = "", double from = 0, double to = double.MaxValue, bool academic = true, bool nonAcademic = true)
        {
            return Ok(await _courseService.SearchForCources(subject, semester, from, to, pageSize, pageindex, academic, nonAcademic));
        }


        [Authorize]
        [HttpGet("by-id")]
        public async Task<IActionResult> GetCourse(string id)
        {
            var course = await _courseService.GetCourse(id);
            return course != null ? Ok(course) : throw new Exception("course not found");
        }


        [Authorize]
        [HttpGet("teacher-courses")]
        public async Task<IActionResult> GetCoursesByTeacherId(string teacherId)
        {
            return Ok(await _courseService.GetCoursesByTeacherId(teacherId));
        }


        [HttpGet("count")]
        public async Task<IActionResult> GetNumberOfCourses()
        {
            return Ok(await _courseService.GetNumberOfCourses());
        }


        [Authorize]
        [HttpGet("students-count-in-course")]
        public async Task<IActionResult> GetStudentCountInCourse(string courseId)
        {
            return Ok(await _courseService.GetStudentCountInCourse(courseId));
        }
    }
}
