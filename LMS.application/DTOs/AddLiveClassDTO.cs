﻿namespace LMS.Application.DTOs
{
    public class AddLiveClassDTO
    {
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }
        public double Duration { get; set; }
        public string Link { get; set; }
        public string CourserId { get; set; }
    }
}
