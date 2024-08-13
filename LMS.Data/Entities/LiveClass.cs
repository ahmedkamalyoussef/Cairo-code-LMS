using LMS.Data.Entities;
using LMS.Domain.Consts;
using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities
{
    public class LiveClass
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }
        public double Duration { get; set; }
        public string Link { get; set; }

        public Status Status
        {
            get
            {
                // Egypt Standard Time
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

                // Convert to Egypt local time
                var now = TimeZoneInfo.ConvertTime(DateTime.Now, egyptTimeZone);

                if (now >= StartDateTime.AddHours(Duration))
                    return Status.Finished;
                if (now >= StartDateTime)
                    return Status.Started;
                if (now >= StartDateTime.AddMinutes(-1))
                    return Status.Almost;
                return Status.Pinding;
            }
        }

        public string CourserId { get; set; }
        public Course Course { get; set; }
        public string CreatorId { get; set; }
        public Teacher Creator { get; set; }
    }
}
