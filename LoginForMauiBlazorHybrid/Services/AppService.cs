using DevExpress.XtraReports.ErrorPanel.Native;
using LoginForMauiBlazorHybrid.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace LoginForMauiBlazorHybrid.Services
{
    public class AppService : IAppService
    {
        public async Task<MainResponse> AuthenticateUser(LoginModel loginModel)
        {
            var returnResponse = new MainResponse();
            using (var client = new HttpClient())
            {
                var url = $"{Setting.BaseUrl}{APIs.AuthenticateUser}";

                var serializedStr = JsonConvert.SerializeObject(loginModel);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<MainResponse>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<List<StudentModel>> GetAllStudents()
        {
            var returnResponse = new List<StudentModel>();
            using (var client = new HttpClient())
            {
                var url = $"{Setting.BaseUrl}{APIs.GetAllStudents}";

                var response = await client.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    bool isTokenRefreshed = await RefreshToken();
                    if (isTokenRefreshed) return await GetAllStudents();
                }
                else
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string contentStr = await response.Content.ReadAsStringAsync();
                        var mainResponse = JsonConvert.DeserializeObject<MainResponse>(contentStr);
                        if (mainResponse.IsSuccess)
                        {
                            returnResponse = JsonConvert.DeserializeObject<List<StudentModel>>(mainResponse.Content.ToString());
                        }
                    }
                }

            }
            return returnResponse;
        }

        public async Task<bool> RefreshToken()
        {
            bool isTokenRefreshed = false;
            using (var client = new HttpClient())
            {
                var url = $"{Setting.BaseUrl}{APIs.RefreshToken}";

                var serializedStr = JsonConvert.SerializeObject(new AuthenticationRequestAndResponse
                {
                    RefreshToken = Setting.UserBasicDetail.RefreshToken,
                    AccessToken = Setting.UserBasicDetail.AccessToken
                });

                try
                {
                    var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string contentStr = await response.Content.ReadAsStringAsync();
                        var mainResponse = JsonConvert.DeserializeObject<MainResponse>(contentStr);
                        if (mainResponse.IsSuccess)
                        {
                            var tokenDetails = JsonConvert.DeserializeObject<AuthenticationRequestAndResponse>(mainResponse.Content.ToString());
                            Setting.UserBasicDetail.AccessToken = tokenDetails.AccessToken;
                            Setting.UserBasicDetail.RefreshToken = tokenDetails.RefreshToken;

                            string userDetailsStr = JsonConvert.SerializeObject(Setting.UserBasicDetail);
                            await SecureStorage.SetAsync(nameof(Setting.UserBasicDetail), userDetailsStr);
                            isTokenRefreshed = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }


            }
            return isTokenRefreshed;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> RegisterUser(RegistrationModel registerUser)
        {
            string errorMessage = string.Empty;
            bool isSuccess = false;
            using (var client = new HttpClient())
            {
                var url = $"{Setting.BaseUrl}{APIs.RegisterUser}";

                var serializedStr = JsonConvert.SerializeObject(registerUser);
                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                }
                else
                {
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            return (isSuccess, errorMessage);
        }

        public async Task<bool> DeleteStudent(int StudentID)
        {
            var obj = new DeleteStudentDTO { StudentID = StudentID };
            using (var client = new HttpClient())
            {
                var url = $"{Setting.BaseUrl}{APIs.DeleteStudent}";
                var serializedStr = JsonConvert.SerializeObject(obj);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));
                HttpResponseMessage rms = await client.DeleteAsync(url);
                if (rms.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
                else
                {
                    return await Task.FromResult(false);

                }
            }
        }

        public async Task<StudentModel> GetById(int StudentID)
        {
            var returnResponse = new StudentModel();
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{Setting.BaseUrl}{APIs.GetById}{StudentID}";
                    var apiResponse = await client.GetAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);

                        if (deserilizeResponse.IsSuccess)
                        {
                            returnResponse = JsonConvert.DeserializeObject<StudentModel>(deserilizeResponse.Content.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return returnResponse;
        }

        public async Task<bool> AddUpdateStudent(StudentModel student)
        {
            string json = JsonConvert.SerializeObject(student);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            if (student.StudentID == 0)
            {

                var url = $"{Setting.BaseUrl}{APIs.AddStudents}";
                client.BaseAddress = new Uri(url);
                HttpResponseMessage responseMessage = await client.PostAsync("", content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }

            }
            else
            {
                var url = $"{Setting.BaseUrl}{APIs.UpdateStudent}";
                client.BaseAddress = new Uri(url);
                HttpResponseMessage responseMessage = await client.PostAsync("", content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(true);

        }
    }
}
