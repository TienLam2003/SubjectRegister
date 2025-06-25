using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.BLL.Implements;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.Controllers
{
    [Authorize(Roles = "admin")]
    public class SemesterController : CommonController
    {
        private readonly ISemesterBL _semesterBL;
        public SemesterController(ISemesterBL semesterBL)
        {
            _semesterBL = semesterBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var data = _semesterBL.GetAll();
            return PartialView("_Table", data);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(SemesterModel semester)
        {
            var data = _semesterBL.Add(semester);
            return Json(new {data.result, modal = ModalModify(data) });
        }

        public IActionResult Update(int code)
        {
            var student = _semesterBL.GetByCode(code);
            return View(student);
        }

        [HttpPost]
        public IActionResult Update(SemesterModel semester)
        {
            var data = _semesterBL.Update(semester);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int code, bool status)
        {
            var data = _semesterBL.UpdateStatus(code, status);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }

        [HttpPost]
        public IActionResult Delete(int code)
        {
            var data = _semesterBL.Delete(code);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }
    }
}
