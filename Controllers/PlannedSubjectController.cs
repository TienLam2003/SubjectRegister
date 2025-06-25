using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.BLL.Implements;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.Controllers
{
    [Authorize(Roles = "admin")]
    public class PlannedSubjectController : CommonController
    {
        private readonly IPlannedSubjectBL _plannedSubjectBL;
        private readonly ISemesterBL _semesterBL;
        private readonly ISetTimePlanSchedule _setTimePlanSchedule;
        public PlannedSubjectController(IPlannedSubjectBL plannedSubjectBL, ISemesterBL semesterBL, ISetTimePlanSchedule setTimePlanSchedule)
        {
            _plannedSubjectBL = plannedSubjectBL;
            _semesterBL = semesterBL;
            _setTimePlanSchedule = setTimePlanSchedule;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPagination(int pageActive)
        {
            var data = _plannedSubjectBL.GetPagination(pageActive);
            return PartialView("_Table", data);
        }

        public IActionResult GetDepartmentsWithSubjects(int semesterCode)
        {
            var data = _plannedSubjectBL.GetDepartmentsWithSubjects(semesterCode);
            // Chuyển đổi dữ liệu thành cấu trúc Select2
            var select2Data = data.Select(department => new
            {
                text = department.DepartmentName, // Tiêu đề nhóm (Khoa)
                children = department.Subjects.Select(subject => new
                {
                    id = subject.SubjectCode, // Giá trị của môn học
                    text = subject.SubjectName // Tên hiển thị của môn học
                }).ToList()
            });
            return Json(select2Data);
        }

        public IActionResult Add()
        {
            ViewBag.Semester = _semesterBL.GetByStatus();
            return View();
        }

        [HttpPost]
        public IActionResult Add(PlannedSubjectModel plannedSubject)
        {
            var data = _plannedSubjectBL.Add(plannedSubject);
            return Ok(new { data.result, modal = ModalModify(data) });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTime(PlanScheduleModel model)
        {
            if(model.PlanDeadline >= model.PlanStart)
            {
                await _setTimePlanSchedule.UpdateTimePlanSchedule(model);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Update(string code)
        {
            var plannedSubject = _plannedSubjectBL.GetByCode(code);
            return View(plannedSubject);
        }

        [HttpPost]
        public IActionResult Update(PlannedSubjectModel plannedSubject)
        {
            var data = _plannedSubjectBL.Update(plannedSubject);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }

        [HttpPost]
        public IActionResult Delete(int code)
        {
            var data = _plannedSubjectBL.Delete(code);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }
    }
}
