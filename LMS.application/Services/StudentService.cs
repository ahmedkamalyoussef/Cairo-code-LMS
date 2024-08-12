using LMS.Application.Helpers;
using LMS.Application.Interfaces;
using LMS.Data.Entities;
using LMS.Data.IGenericRepository_IUOW;
using LMS.Domain.Consts;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.Services
{
    public class StudentService(IUnitOfWork unitOfWork, IUserHelpers userHelpers) : IStudentService
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserHelpers _userHelpers = userHelpers;
        public async Task<int> GetStudentsCount()
        {
            return await _unitOfWork.Students.CountAsync();
        }
        public async Task<int> EnrolledStudentsCount()
        {
            return await _unitOfWork.StudentCourses.CountAsync();
        }

        public async Task<bool> EditStudentImage(IFormFile image)
        {
            Student user = (Student)await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldImgPath = user.Image;
            user.Image = await _userHelpers.AddFileAsync(image, Folder.Profile);


            await _unitOfWork.Users.UpdateAsync(user);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                if (oldImgPath != null)
                    await _userHelpers.DeleteFileAsync(oldImgPath, Folder.Profile);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteStudentPictureAsync()
        {
            Student user = (Student)await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldImgPath = user.Image;
            user.Image = null;
            await _unitOfWork.Users.UpdateAsync(user);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                if (oldImgPath != null)
                    await _userHelpers.DeleteFileAsync(oldImgPath, Folder.Profile);
                return true;
            }
            return false;
        }
    }
}
