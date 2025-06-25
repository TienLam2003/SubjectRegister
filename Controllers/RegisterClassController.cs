using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.BLL.Interfaces;

namespace SubjectRegister.Controllers
{
    [Authorize(Roles = "student")]
    public class RegisterClassController : CommonController
    {
        private readonly IRegisterClassBL _registerClassBL;
        public RegisterClassController(IRegisterClassBL registerClassBL) 
        {
            _registerClassBL = registerClassBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddRegister() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRegister(int classCode)
        {
            string studentCode = HttpContext.Session.GetString("Code") ?? "";
            var data = _registerClassBL.AddRegister(classCode, studentCode);
            return PartialView("_ModelNotifyPartial", data);
        }

        /// <summary>
        /// Lấy danh sách lớp học tương ứng với kế hoạch học tập của sv
        /// </summary>
        /// <returns></returns>
        public IActionResult GetClassByStudent()
        {
            string studentCode = HttpContext.Session.GetString("Code") ?? "";
            var data = _registerClassBL.GetClassByStudent(studentCode);
            return PartialView("_TableRegister", data);
        }

        /// <summary>
        /// Lấy lịch học của sinh viên
        /// </summary>
        /// <returns></returns>
        public IActionResult GetScheduleByStudent()
        {
            string studentCode = HttpContext.Session.GetString("Code") ?? "";
            var data = _registerClassBL.GetScheduleByStudent(studentCode);
            return PartialView("_TableSchedule", data);
        }

        [HttpPost]
        public IActionResult Delete(int code) // Có thể thêm StudentCode để ràng đúng sinh viên xóa
        {
            var data = _registerClassBL.Delete(code);
            return Ok(data);
        }
    }
}
