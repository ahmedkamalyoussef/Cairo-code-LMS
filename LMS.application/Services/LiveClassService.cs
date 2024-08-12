using LMS.Application.DTOs;
using LMS.Application.Interfaces;

namespace LMS.Application.Services
{
    public class LiveClassService : ILiveClassService
    {
        public Task<bool> CreateLiveClass(AddLiveClassDTO liveClassDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLiveClass(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<LiveClassDTO>> GetCurrentTeacherLiveClasses()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLiveClass(string id, AddLiveClassDTO liveClassDTO)
        {
            throw new NotImplementedException();
        }
    }
}
