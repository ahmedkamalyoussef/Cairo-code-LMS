using AutoMapper;
using LMS.Application.DTOs;
using LMS.Application.Helpers;
using LMS.Application.Interfaces;
using LMS.Data.IGenericRepository_IUOW;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Application.Services
{
    public class LiveClassService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers) : ILiveClassService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserHelpers _userHelpers = userHelpers;

        public async Task<bool> CreateLiveClass(AddLiveClassDTO liveClassDTO)
        {
            var teacher = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            var liveCourse = _mapper.Map<LiveClass>(liveClassDTO);
            liveCourse.CreatorId = teacher.Id;
            await _unitOfWork.LiveClasses.AddAsync(liveCourse);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> UpdateLiveClass(string id, EditLiveClassDTO liveClassDTO)
        {
            _ = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            var liveClass = await _unitOfWork.LiveClasses.FindFirstAsync(c => c.Id == id) ?? throw new Exception("liveClass not found");
            _mapper.Map(liveClassDTO, liveClass);
            await _unitOfWork.LiveClasses.UpdateAsync(liveClass);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> DeleteLiveClass(string id)
        {
            _ = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            var liveClass = await _unitOfWork.LiveClasses.FindFirstAsync(c => c.Id == id) ?? throw new Exception("liveClass not found");
            await _unitOfWork.LiveClasses.RemoveAsync(liveClass);
            return await _unitOfWork.SaveAsync() > 0;
        }


        public async Task<Tuple<List<LiveClassDTO>, int>> GeStudentLiveClasses(int pageSize, int pageindex)
        {
            var student = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            var studentCourses = await _unitOfWork.StudentCourses.FindAsync(c => c.StudentId == student.Id
                , null, null, includes: [c => c.Course],
                thenIncludes: [query => query.Include(c => c.Course).ThenInclude(course => course.LiveClasses)]);
            var courses = studentCourses.Select(c => c.Course);
            var liveClasses = courses.SelectMany(c => c.LiveClasses).Skip((pageindex - 1) * pageSize).Take(pageSize);
            var liveClassResult = _mapper.Map<IEnumerable<LiveClassDTO>>(liveClasses).ToList();
            return Tuple.Create(liveClassResult, liveClassResult.Count);
        }

        public async Task<Tuple<List<LiveClassDTO>, int>> GetCurrentTeacherLiveClasses(int pageSize, int pageindex)
        {
            var teacher = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            var liveClasses = await _unitOfWork.LiveClasses.FindWithPaginationAsync(pageSize, pageindex, l => l.CreatorId == teacher.Id);
            var liveClassesResult = _mapper.Map<IEnumerable<LiveClassDTO>>(liveClasses).ToList();
            return Tuple.Create(liveClassesResult, liveClassesResult.Count);
        }
    }
}
