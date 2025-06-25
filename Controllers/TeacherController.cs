using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Models;

namespace SubjectRegister.Controllers
{
    [Authorize(Roles = "admin")]
    public class TeacherController : CommonController
    {
        private readonly ITeacherBL _teacherBL;
        public TeacherController(ITeacherBL teacherBL)
        {
            _teacherBL = teacherBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPagination(int pageActive)
        {
            var data = _teacherBL.GetPagination(pageActive);
            return PartialView("_Table", data);
        }

        public IActionResult GetBySearch(TeacherSearchModel modelSearch)
        {
            var data = _teacherBL.GetBySearch(modelSearch);
            ViewBag.CodeSearch = modelSearch.TeacherCode;
            ViewBag.NameSearch = modelSearch.Fullname;
            return PartialView("_Table", data);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(TeacherModel teacher)
        {
            var data = _teacherBL.Add(teacher);
            return Json(new { data.result, modal = ModalModify(data) });
        }

        public IActionResult Update(string code)
        {
            var student = _teacherBL.GetByCode(code);
            return View(student);
        }

        [HttpPost]
        public IActionResult Update(TeacherModel teacher)
        {
            var data = _teacherBL.Update(teacher);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }

        [HttpPost]
        public IActionResult Delete(string code)
        {
            var data = _teacherBL.Delete(code);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }
    }
}
