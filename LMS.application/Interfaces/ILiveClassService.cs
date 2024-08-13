using LMS.Application.DTOs;

namespace LMS.Application.Interfaces
{
    public interface ILiveClassService
    {
        public Task<List<LiveClassDTO>> GetCurrentTeacherLiveClasses();
        public Task<List<LiveClassDTO>> GeStudentLiveClasses();
        public Task<bool> CreateLiveClass(AddLiveClassDTO liveClassDTO);
        public Task<bool> UpdateLiveClass(string id, EditLiveClassDTO liveClassDTO);
        public Task<bool> DeleteLiveClass(string id);
    }
}
