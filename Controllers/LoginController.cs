using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Models;
using System.Security.Claims;

namespace SubjectRegister.Controllers
{
    public class LoginController : Controller
    {
        public readonly ILoginBL _loginBL;
        public LoginController(ILoginBL loginBL)
        {
            _loginBL = loginBL;
        }

        public IActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Auth(UserModel user)
        {
            bool result = await _loginBL.Auth(user);
            if (result)
                return Ok(new { success = true });
            else
                return Ok(new { success = false });
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Auth");
        }   

        public IActionResult Profile()
        {
            string code = HttpContext.Session.GetString("Code") ?? "";
            string roleCode = HttpContext.Session.GetString("RoleCode") ?? "";
            if(roleCode == "student")
            {
                var data = _loginBL.GetStudentByCode(code);
                return View("~/Views/Login/StudentProfile.cshtml" ,data);
            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        //public IActionResult ForgotPassword(string email)
        //{
        //    // Tạo OTP ngẫu nhiên
        //    var otpCode = new Random().Next(100000, 999999).ToString();
        //    MailContent content = new MailContent
        //    {
        //        To = email,
        //        Subject = "Xác thực địa chỉ email",
        //        Body = $"OTP xác nhận đăng ký tài khoản ChatZ của bạn là {otpCode}"
        //    };

        //    //Lưu OTP vào Redis
        //    await _redis.StringSetAsync(model.Email, otpCode, TimeSpan.FromMinutes(5));

        //    //Gửi mã OTP qua mail
        //    await _sendMailService.SendMail(content);
        //    return View();
        //}
    }
}
