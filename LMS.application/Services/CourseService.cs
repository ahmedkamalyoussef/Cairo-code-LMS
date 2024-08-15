using AutoMapper;
using LMS.Application.DTOs;
using LMS.Application.Helpers;
using LMS.Application.Interfaces;
using LMS.Data.Consts;
using LMS.Data.Entities;
using LMS.Data.IGenericRepository_IUOW;
using LMS.Domain.Consts;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.Services
{
    public class CourseService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers) : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserHelpers _userHelpers = userHelpers;
        public async Task<List<CourseResultDTO>> GetTopAcademicCourses(int count)
        {
            var topAcademicCourses = await _unitOfWork.AcademicCourses.FindTopAsync(c => c.StudentCourses.Count, includes: [c => c.Teacher], take: count);
            var coursesResult = _mapper.Map<IEnumerable<CourseResultDTO>>(topAcademicCourses).ToList();
            return coursesResult;
        }
        public async Task<List<CourseResultDTO>> GetTopNonAcademicCourses(int count)
        {
            var topNonAcademicCourses = await _unitOfWork.NonAcademicCourses.FindTopAsync(c => c.StudentCourses.Count, includes: [c => c.Teacher], take: count);
            var coursesResult = _mapper.Map<IEnumerable<CourseResultDTO>>(topNonAcademicCourses).ToList();
            return coursesResult;
        }
        public async Task<bool> CreateCourse(CourseDTO courseDto, IFormFile img)
        {
            var teacher = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            Course course = courseDto.Category switch
            {
                Category.Academic => _mapper.Map<AcademicCourse>(courseDto),
                Category.NonAcademic => _mapper.Map<NonAcademicCourse>(courseDto),
            };
            course.TeacherId = teacher.Id;
            if (img != null)
                course.Image = await _userHelpers.AddFileAsync(img, Folder.Image);
            if (course.Category == Category.Academic)
                await _unitOfWork.AcademicCourses.AddAsync((AcademicCourse)course);
            else
                await _unitOfWork.NonAcademicCourses.AddAsync((NonAcademicCourse)course);
            return await _unitOfWork.SaveAsync() > 0;
        }
        public async Task<bool> UpdateCourse(string id, EditCourseDTO courseDTO, IFormFile? img)
        {
            _ = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            Course course = courseDTO.Category switch
            {
                Category.Academic => await _unitOfWork.AcademicCourses.FindFirstAsync(c => c.Id == id) ?? throw new Exception("course not found"),
                Category.NonAcademic => await _unitOfWork.NonAcademicCourses.FindFirstAsync(c => c.Id == id) ?? throw new Exception("course not found")
            };
            var oldImgPath = course.Image;
            _mapper.Map(courseDTO, course);
            if (img != null)
            {
                course.Image = await _userHelpers.AddFileAsync(img, Folder.Image);
            }
            if (course.Category == Category.Academic)
                await _unitOfWork.AcademicCourses.UpdateAsync((AcademicCourse)course);
            else
                await _unitOfWork.NonAcademicCourses.UpdateAsync((NonAcademicCourse)course);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                if (oldImgPath != null && img != null)
                    await _userHelpers.DeleteFileAsync(oldImgPath, Folder.Image);
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteCourse(string id)
        {
            _ = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            var course = await _unitOfWork.Courses.FindFirstAsync(c => c.Id == id, includes: [c => c.Books, c => c.Lectures]) ?? throw new Exception("course not found");
            var oldImgPath = course.Image;

            await _unitOfWork.Courses.RemoveAsync(course);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                if (oldImgPath != null)
                    await _userHelpers.DeleteFileAsync(oldImgPath, Folder.Image);
                foreach (Book book in course.Books)
                    await _userHelpers.DeleteFileAsync(book.BookUrl, Folder.Book);
                foreach (Lecture lecture in course.Lectures)
                    await _userHelpers.DeleteFileAsync(lecture.LectureUrl, Folder.Lecture);
                return true;
            }
            return false;
        }

        public async Task<bool> EnrollingStudentInCourse(string StudentEmail, string courseCode)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            var student = await _unitOfWork.Students.FindFirstAsync(s => s.Email == StudentEmail) ?? throw new Exception("student not found");
            var course = await _unitOfWork.Courses.FindFirstAsync(c => c.Code == courseCode) ?? throw new Exception("course not found");

            var newStudentCourse = new StudentCourse { CourseId = course.Id, StudentId = student.Id };
            await _unitOfWork.StudentCourses.AddAsync(newStudentCourse);
            return await _unitOfWork.SaveAsync() > 0;

        }

        public async Task<List<CourseResultDTO>> GetAcademicCourses()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var courses = await _unitOfWork.AcademicCourses.GetAllAsync(orderBy: course => course.Name,
            direction: OrderDirection.Ascending,
            includes:
            [
                course => course.Teacher,
            ]);
            var coursesResult = _mapper.Map<IEnumerable<CourseResultDTO>>(courses).ToList();
            foreach (var course in coursesResult)
            {
                var evaluations = await _unitOfWork.Evaluations.FindAsync(e => e.CourseId == course.Id);
                course.Evaluation = CalculateAverageRate(evaluations.ToList());
            }
            if (currentUser != null)
            {
                foreach (var course in coursesResult)
                {
                    var studentCuorse = await _unitOfWork.StudentCourses.FindFirstAsync(sc => sc.CourseId == course.Id && sc.StudentId == currentUser.Id);
                    if (studentCuorse != null) course.IsEnrolled = true;
                    else course.IsEnrolled = false;
                }
            }
            return coursesResult;
        }

        public async Task<List<CourseResultDTO>> GetNonAcademicCourses()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var courses = await _unitOfWork.NonAcademicCourses.GetAllAsync(orderBy: course => course.Name,
                direction: OrderDirection.Ascending,
                includes:
                [
                    course => course.Teacher,
                ]);
            var coursesResult = _mapper.Map<IEnumerable<CourseResultDTO>>(courses).ToList();
            foreach (var course in coursesResult)
            {
                var evaluations = await _unitOfWork.Evaluations.FindAsync(e => e.CourseId == course.Id);
                course.Evaluation = CalculateAverageRate(evaluations.ToList());
            }
            if (currentUser != null)
            {
                foreach (var course in coursesResult)
                {
                    var studentCuorse = await _unitOfWork.StudentCourses.FindFirstAsync(sc => sc.CourseId == course.Id && sc.StudentId == currentUser.Id);
                    if (studentCuorse != null) course.IsEnrolled = true;
                    else course.IsEnrolled = false;
                }
            }
            return coursesResult;
        }

        public async Task<CourseResultDTO> GetCourse(string id)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var course = await _unitOfWork.Courses.FindFirstAsync(c => c.Id == id,
            includes:
            [
                course => course.Teacher,
                cource => cource.Evaluations

            ]);
            var coursesResult = _mapper.Map<CourseResultDTO>(course);
            coursesResult.Evaluation = CalculateAverageRate(course.Evaluations.ToList());
            if (currentUser != null)
            {
                var studentCuorse = await _unitOfWork.StudentCourses.FindFirstAsync(sc => sc.CourseId == course.Id && sc.StudentId == currentUser.Id);
                if (studentCuorse != null) coursesResult.IsEnrolled = true;
                else coursesResult.IsEnrolled = false;
            }
            return coursesResult;
        }

        public async Task<List<CourseResultDTO>> GetCoursesByTeacherId(string id)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var courses = await _unitOfWork.Courses.FindAsync(c => c.TeacherId == id,
            orderBy: course => course.Name,
            direction: OrderDirection.Ascending,
            includes:
            [
                c => c.Teacher,
            ]);
            var coursesResult = _mapper.Map<IEnumerable<CourseResultDTO>>(courses).ToList();
            foreach (var course in coursesResult)
            {
                var evaluations = await _unitOfWork.Evaluations.FindAsync(e => e.CourseId == course.Id);
                course.Evaluation = CalculateAverageRate(evaluations.ToList());
            }
            if (currentUser != null)
            {
                foreach (var course in coursesResult)
                {
                    var studentCuorse = await _unitOfWork.StudentCourses.FindFirstAsync(sc =>
                        sc.CourseId == course.Id && sc.StudentId == currentUser.Id);
                    if (studentCuorse != null) course.IsEnrolled = true;
                    else course.IsEnrolled = false;
                }
            }
            return coursesResult;
        }

        public async Task<List<CourseResultDTO>> GetCoursesByStudentId(string id)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var studentCourses = await _unitOfWork.StudentCourses.FindAsync(c => c.StudentId == id,
                includes:
                [
                    c => c.Course,
                ]);
            var courses = studentCourses.Select(c => c.Course);
            var coursesResult = _mapper.Map<IEnumerable<CourseResultDTO>>(courses).ToList();
            foreach (var course in coursesResult)
            {
                var evaluations = await _unitOfWork.Evaluations.FindAsync(e => e.CourseId == course.Id);
                course.Evaluation = CalculateAverageRate(evaluations.ToList());
            }
            if (currentUser != null)
            {
                foreach (var course in coursesResult)
                {
                    var studentCuorse = await _unitOfWork.StudentCourses.FindFirstAsync(sc =>
                        sc.CourseId == course.Id && sc.StudentId == currentUser.Id);
                    if (studentCuorse != null) course.IsEnrolled = true;
                    else course.IsEnrolled = false;
                }
            }
            return coursesResult;
        }

        public async Task<int> GetNumberOfCourses()
        {
            return await _unitOfWork.Courses.CountAsync();
        }

        public async Task<int> GetStudentCountInCourse(string courseId)
        {
            var studentCourses = await _unitOfWork.StudentCourses.FindAsync(sc => sc.CourseId == courseId);
            return studentCourses.Count();
        }

        public async Task<Tuple<List<CourseResultDTO>, int>> SearchForCources(string subject, string semester, string level, double from, double to, int pageSize, int pageIndex, bool academic = true, bool nonAcademic = true)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            List<Course> courses = [];
            if (academic)
            {
                var filteredAcademic = await _unitOfWork.AcademicCourses.FilterAsync(pageSize, pageIndex, [c =>
                            c.MaterialName.Contains(subject) || c.Name.Contains(subject)
                        || subject.Contains(c.MaterialName) || subject.Contains(c.Name)||c.Level.Contains(level)||level.Contains(c.Level),
                    c => semester.Contains(c.Semester) || c.Semester.Contains(semester),
                    c => c.Price >= from && c.Price <= to
                ],
                orderBy: course => course.Name,
                direction: OrderDirection.Descending,
                includes:
                [
                    c => c.Teacher
                ]);
                courses.AddRange(filteredAcademic.ToList());
            }
            if (nonAcademic)
            {
                var filteredNonAcademic = await _unitOfWork.NonAcademicCourses.FilterAsync(pageSize, pageIndex, [c =>c.Name.Contains(subject)
                            || subject.Contains(c.Name),
                        c => c.Price >= from && c.Price <= to
                    ],
                    orderBy: course => course.Name,
                    direction: OrderDirection.Descending,
                    includes:
                    [
                        c => c.Teacher
                    ]);
                courses.AddRange(filteredNonAcademic.ToList());
            }
            var coursesResult = _mapper.Map<IEnumerable<CourseResultDTO>>(courses).ToList();
            foreach (var course in coursesResult)
            {
                var evaluations = await _unitOfWork.Evaluations.FindAsync(e => e.CourseId == course.Id);
                course.Evaluation = CalculateAverageRate(evaluations.ToList());
            }
            if (currentUser != null)
            {
                foreach (var course in coursesResult)
                {
                    var studentCuorse = await _unitOfWork.StudentCourses.FindFirstAsync(sc => sc.CourseId == course.Id && sc.StudentId == currentUser.Id);
                    if (studentCuorse != null) course.IsEnrolled = true;
                    else course.IsEnrolled = false;
                }
            }
            return Tuple.Create(coursesResult, coursesResult.Count);
        }



        #region private methods
        private double CalculateAverageRate(List<Evaluation>? evaluations)
        {
            if (evaluations == null || !evaluations.Any())
            {
                return 0;
            }
            double totalRate = 0;
            foreach (var evaluation in evaluations)
            {
                totalRate += evaluation.Value;
            }
            return totalRate / evaluations.Count();
        }
        #endregion
    }
}
