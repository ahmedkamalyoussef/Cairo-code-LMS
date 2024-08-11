﻿using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs
{
    public class QuestionDTO
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public string ExamId { get; set; }
    }
}
