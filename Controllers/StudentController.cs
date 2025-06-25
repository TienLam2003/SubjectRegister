using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Controllers.Validator;
using SubjectRegister.Models;

namespace SubjectRegister.Controllers
{
    [Authorize(Roles = "admin")]
    public class StudentController : CommonController
    {
        private readonly IStudentBL _studentBL;
        private StudentValidator _validator = new StudentValidator();
        public StudentController(IStudentBL studentBL)
        {
            _studentBL = studentBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPagination(int pageActive)
        {
            var data = _studentBL.GetPagination(pageActive);
            return PartialView("_Table", data);
        }

        public IActionResult GetBySearch(StudentSearchModel modelSearch)
        {
            var data = _studentBL.GetBySearch(modelSearch);
            ViewBag.CodeSearch = modelSearch.StudentCode;
            ViewBag.NameSearch = modelSearch.Fullname;
            return PartialView("_Table", data);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(StudentModel student)
        {
            var validationResult = _validator.Validate(student);
            if (!validationResult.IsValid)
            {
                return PartialView("_ModelNotifyPartial", new QueryResultModel { result = false, message = validationResult.Errors.FirstOrDefault()?.ErrorMessage });
            }

            var data = _studentBL.Add(student);
            return Json(new { data.result, modal = ModalModify(data) });
        }

        public IActionResult Update(string code)
        {
            var student = _studentBL.GetByCode(code);
            return View(student);
        }

        [HttpPost]
        public IActionResult Update(StudentModel student)
        {
            var validationResult = _validator.Validate(student);
            if (!validationResult.IsValid)
            {
                return PartialView("_ModelNotifyPartial", new QueryResultModel { result = false, message = validationResult.Errors.FirstOrDefault()?.ErrorMessage });
            }

            var data = _studentBL.Update(student);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }

        [HttpPost]
        public IActionResult UpdatePassword(string password1, string password2)
        {
            if(password1 != password2)
                return PartialView("_ModelNotifyPartial", new QueryResultModel { result = false, message = "Mật khẩu không trùng khớp"});
            int userCode = HttpContext.Session.GetInt32("UserCode") ?? 0;
            var data = _studentBL.UpdatePassword(password1, userCode);
            return PartialView("_ModelNotifyPartial", data);
        }

        [HttpPost]
        public IActionResult Delete(string code)
        {
            var data = _studentBL.Delete(code);
            if(data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }
    }
}
