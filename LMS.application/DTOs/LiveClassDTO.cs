using LMS.Domain.Consts;

namespace LMS.Application.DTOs
{
    public class LiveClassDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }
        public double Duration { get; set; }
        public string Link { get; set; }
        public Status Status { get; set; }
    }
}
