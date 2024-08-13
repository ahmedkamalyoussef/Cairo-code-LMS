using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController(IStudentService studentService) : ControllerBase
    {
        private readonly IStudentService _studentService = studentService;


        [HttpGet("count")]
        public async Task<IActionResult> GetStudentsCount()
        {
            var count = await _studentService.GetStudentsCount();
            return Ok(count);
        }
        [HttpGet("enrolled-count")]
        public async Task<IActionResult> GetEnrolledStudentsCount()
        {
            var count = await _studentService.EnrolledStudentsCount();
            return Ok(count);
        }
        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage(IFormFile newImage)
        {
            bool result = await _studentService.EditStudentImage(newImage);
            return result ? Ok("updated successfully") : BadRequest("failed to update");

        }


        [HttpDelete("image")]
        public async Task<IActionResult> DeleteImage()
        {
            var result = await _studentService.DeleteStudentPictureAsync();
            return result ? Ok("deleted successfully") : BadRequest("failed to delete");

        }
    }
}
