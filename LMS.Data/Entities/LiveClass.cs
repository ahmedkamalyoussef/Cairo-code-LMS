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
        public DateTime EndDateTime { get; set; }
        public string Link { get; set; }

        public Status Status
        {
            get
            {
                var now = DateTime.Now;
                if (now >= EndDateTime)
                    return Status.Finished;
                if (now >= StartDateTime)
                    return Status.Started;
                if (now >= StartDateTime.AddMinutes(-1))
                    return Status.Almost;
                return Status.Pinding;
            }
        }
        public string CreatorId { get; set; }
        public Teacher Creator { get; set; }
    }
}
