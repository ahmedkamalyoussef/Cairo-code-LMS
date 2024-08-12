using Microsoft.AspNetCore.Http;

namespace LMS.Application.Interfaces
{
    public interface IStudentService
    {
        public Task<int> GetStudentsCount();
        public Task<int> EnrolledStudentsCount();
        public Task<bool> EditStudentImage(IFormFile image);
        public Task<bool> DeleteStudentPictureAsync();
    }
}
