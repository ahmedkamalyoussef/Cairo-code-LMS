using LMS.Application.DTOs;

namespace LMS.Application.Interfaces
{
    public interface ILiveClassService
    {
        public Task<Tuple<List<LiveClassDTO>, int>> GetCurrentTeacherLiveClasses(int pageSize, int pageindex);
        public Task<Tuple<List<LiveClassDTO>, int>> GeStudentLiveClasses(int pageSize, int pageindex);
        public Task<bool> CreateLiveClass(AddLiveClassDTO liveClassDTO);
        public Task<bool> UpdateLiveClass(string id, EditLiveClassDTO liveClassDTO);
        public Task<bool> DeleteLiveClass(string id);
    }
}
