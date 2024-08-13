﻿using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs
{
    public class EditCourseDTO
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? MaterialName { get; set; }
        public string Level { get; set; }
        public string Semester { get; set; }
        public double Price { get; set; }
        public string? Content { get; set; }
        public string? Details { get; set; }
    }
}
