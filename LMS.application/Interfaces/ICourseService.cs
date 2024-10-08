﻿using LMS.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.Interfaces
{
    public interface ICourseService
    {
        public Task<CourseResultDTO> GetCourse(string id);
        public Task<List<CourseResultDTO>> GetNonAcademicCourses();
        public Task<List<CourseResultDTO>> GetAcademicCourses();
        public Task<List<CourseResultDTO>> GetTopAcademicCourses(int count);
        Task<List<CourseResultDTO>> GetTopNonAcademicCourses(int count);
        public Task<bool> CreateCourse(CourseDTO course, IFormFile file);
        public Task<bool> UpdateCourse(string id, EditCourseDTO course, IFormFile? file);
        public Task<bool> DeleteCourse(string id);
        public Task<int> GetNumberOfCourses();
        public Task<Tuple<List<CourseResultDTO>, int>> GetCoursesByTeacherId(string id, int pageSize, int pageindex);
        public Task<List<CourseResultDTO>> GetCoursesByStudentId(string id);

        public Task<Tuple<List<CourseResultDTO>, int>> SearchForCources(string subject, string semester, string level, double from, double to, int pageSize, int pageIndex, bool academic, bool nonAcademic);
        public Task<bool> EnrollingStudentInCourse(string StudentEmail, string CourseCode);
        public Task<int> GetStudentCountInCourse(string courseId);

    }
}
