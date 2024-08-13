using AutoMapper;
using LMS.Application.DTOs;
using LMS.Application.Helpers;
using LMS.Application.Interfaces;
using LMS.Data.IGenericRepository_IUOW;
using LMS.Domain.Entities;

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


        public Task<List<LiveClassDTO>> GeStudentLiveClasses()
        {
            throw new NotImplementedException();
        }

        public async Task<List<LiveClassDTO>> GetCurrentTeacherLiveClasses()
        {
            var teacher = await _userHelpers.GetCurrentUserAsync() ?? throw new Exception("user not found");
            var liveClasses = await _unitOfWork.LiveClasses.FindAsync(l => l.CreatorId == teacher.Id);
            var liveClassesResult = _mapper.Map<IEnumerable<LiveClassDTO>>(liveClasses).ToList();
            return liveClassesResult;
        }
    }
}
