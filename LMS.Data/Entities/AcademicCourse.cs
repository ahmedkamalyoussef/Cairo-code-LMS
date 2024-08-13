using LMS.Data.Entities;

namespace LMS.Domain.Entities
{
    public class AcademicCourse : Course
    {
        public string? MaterialName { get; set; }
        public string Level { get; set; }
        public string Semester { get; set; }
    }
}
