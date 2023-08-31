using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginForMauiBlazorHybrid.Models
{
    public class APIs
    {
        public const string AuthenticateUser = "/api/User/AuthenticateUser";
        public const string RegisterUser = "/api/User/RegisterUser";
        public const string RefreshToken = "/api/User/RefreshToken";

        public const string GetAllStudents = "/api/Students/GetAllStudent";
        public const string AddStudents = "/api/Students/AddStudent";
        public const string DeleteStudent = "/api/Students/DeleteStudent";
        public const string UpdateStudent = "/api/Students/UpdateStudent/";
        public const string GetById = "/api/Students/GetStudentByStudentID/";

    }
}
