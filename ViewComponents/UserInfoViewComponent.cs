using Microsoft.AspNetCore.Mvc;
using SubjectRegister.Common;
using SubjectRegister.DAL;

namespace SubjectRegister.ViewComponents
{
    public class UserInfoViewComponent : ViewComponent
    {
        private readonly LoginDAO _loginDAO;
        public UserInfoViewComponent(LoginDAO loginDAO)
        {
            _loginDAO = loginDAO;
        }

        /// <summary>
        /// Dự định session chỉ lưu 1 mã cho 1 user và component này truy vấn render giao diện dùng chung
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            var userInfo = new UserInfo
            {
                Fullname = HttpContext.Session.GetString("Fullname") ?? "",
                ImagePath = HttpContext.Session.GetString("ImagePath") ?? "",
                RoleName = HttpContext.Session.GetString("RoleName") ?? ""
            };
            return View(viewName, userInfo);
        }
    }
}
