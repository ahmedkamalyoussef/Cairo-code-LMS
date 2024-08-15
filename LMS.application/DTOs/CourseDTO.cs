using LMS.Domain.Consts;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.DTOs
{
    public class CourseDTO
    {
        public string Name { get; set; }
        public string? MaterialName { get; set; }
        IFormFile CourseImage {  get; set; }
        public string? Level { get; set; }
        public string? Semester { get; set; }
        public double Price { get; set; }
        public string? Content { get; set; }
        public string? Details { get; set; }
        public Category Category { get; set; }

    }
}
