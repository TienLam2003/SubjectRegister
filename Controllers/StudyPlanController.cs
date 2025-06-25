using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Models;

namespace SubjectRegister.Controllers
{
    [Authorize(Roles = "student")]
    public class StudyPlanController : CommonController
    {
        private readonly IStudyPlanBL _studyPlanBL;
        public StudyPlanController(IStudyPlanBL studyPlanBL)
        {
            _studyPlanBL = studyPlanBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetByStudent()
        {
            var studentCode = HttpContext.Session.GetString("Code") ?? "";
            var data = _studyPlanBL.GetByStudent(studentCode);
            return PartialView("_Table", data);
        }

        public IActionResult Add()
        {
            string departmentCode = HttpContext.Session.GetString("DepartmentCode") ?? string.Empty;
            var data = _studyPlanBL.GetSubjectAsPlanedSubject(departmentCode);
            return View(data);
        }

        [HttpPost]
        public IActionResult Add(StudyPlanModel model)
        {
            model.StudentCode = HttpContext.Session.GetString("Code") ?? "";
            var data = _studyPlanBL.Add(model);
            return PartialView("_ModelNotifyPartial", data);
        }

        public IActionResult Update(string code)
        {
            var plannedSubject = _studyPlanBL.GetByCode(code);
            return View(plannedSubject);
        }

        [HttpPost]
        public IActionResult Update(StudyPlanModel model)
        {
            var data = _studyPlanBL.Update(model);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }

        [HttpPost]
        public IActionResult Delete(int code)
        {
            var data = _studyPlanBL.Delete(code);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }
    }
}
