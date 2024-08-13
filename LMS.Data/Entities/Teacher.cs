using LMS.Domain.Entities;

namespace LMS.Data.Entities
{
    public class Teacher : ApplicationUser
    {
        public string? Image { get; set; }
        public ICollection<Course>? Courses { get; set; }
        public ICollection<LiveClass>? LiveClasses { get; set; }

    }
}
