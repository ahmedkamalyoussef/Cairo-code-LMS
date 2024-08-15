using LMS.Application.DTOs;

namespace LMS.Application.Interfaces
{
    public interface ILiveClassService
    {
        public Task<List<LiveClassDTO>> GetCurrentTeacherLiveClasses(int pageSize, int pageindex);
        public Task<List<LiveClassDTO>> GeStudentLiveClasses(int pageSize, int pageindex);
        public Task<bool> CreateLiveClass(AddLiveClassDTO liveClassDTO);
        public Task<bool> UpdateLiveClass(string id, EditLiveClassDTO liveClassDTO);
        public Task<bool> DeleteLiveClass(string id);
    }
}
