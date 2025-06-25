using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.DAL;
using SubjectRegister.Models;
using System.Security.Claims;

namespace SubjectRegister.BLL.Implements
{
    public class LoginBL : ILoginBL
    {
        private readonly LoginDAO _loginDAO;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginBL(LoginDAO loginDAO, IHttpContextAccessor httpContextAccessor)
        {
            _loginDAO = loginDAO;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Auth(UserModel user)
        {
            var result = _loginDAO.Auth(user);
            if (result)
            {
                var userInfo = _loginDAO.GetUser(user.Username);

                // Ensure HttpContext and Session are not null before accessing them
                if (_httpContextAccessor.HttpContext?.Session != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("UserCode", userInfo.UserCode);
                    _httpContextAccessor.HttpContext.Session.SetString("RoleCode", userInfo.RoleCode);
                    _httpContextAccessor.HttpContext.Session.SetString("Fullname", userInfo.Fullname);
                    _httpContextAccessor.HttpContext.Session.SetString("ImagePath", userInfo.ImagePath);
                    _httpContextAccessor.HttpContext.Session.SetString("RoleName", userInfo.RoleName);
                    _httpContextAccessor.HttpContext.Session.SetString("DepartmentCode", userInfo.DepartmentCode);
                    _httpContextAccessor.HttpContext.Session.SetString("Code", userInfo.Code);
                }

                // Create Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Code),
                    new Claim(ClaimTypes.Role, userInfo.RoleCode)
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                // Use HttpContextAccessor to access HttpContext
                if (_httpContextAccessor.HttpContext != null)
                {
                    await _httpContextAccessor.HttpContext.SignInAsync("CookieAuth", principal);
                }
            }
            return result;
        }

        public StudentModel GetStudentByCode(string code)
        {
            return _loginDAO.GetStudentByCode(code);
        }

        public TeacherModel GetTeacherByCode(string code)
        {
            return _loginDAO.GetTeacherByCode(code);
        }
    }
}
