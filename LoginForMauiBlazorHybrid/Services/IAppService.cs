using LoginForMauiBlazorHybrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForMauiBlazorHybrid.Services
{
    public interface IAppService
    {
        public Task<bool> RefreshToken();
        public Task<MainResponse> AuthenticateUser(LoginModel loginModel);
        public Task<(bool IsSuccess, string ErrorMessage)> RegisterUser(RegistrationModel registerUser);
        public Task<List<StudentModel>> GetAllStudents();
        public Task<bool>AddUpdateStudent (StudentModel student);
        public Task<bool> DeleteStudent(int StudentID);
        public Task<StudentModel> GetById(int StudentID);

    }
}
