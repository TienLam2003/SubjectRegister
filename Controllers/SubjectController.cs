using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.Controllers
{
    [Authorize(Roles = "admin")]
    public class SubjectController : CommonController
    {
        private readonly ISubjectBL _subjectBL;
        public SubjectController(ISubjectBL subjectBL)
        {
            _subjectBL = subjectBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPagination(int pageActive)
        {
            var data = _subjectBL.GetPagination(pageActive);
            return PartialView("_Table", data);
        }

        public IActionResult GetBySearch(SubjectSearchModel modelSearch)
        {
            var data = _subjectBL.GetBySearch(modelSearch);
            ViewBag.CodeSearch = modelSearch.SubjectCode;
            ViewBag.NameSearch = modelSearch.SubjectName;
            return PartialView("_Table", data);
        }

        public IActionResult GetSubjectByDepartment(string departmentCode)
        {
            var data = _subjectBL.GetSubjectByDepartment(departmentCode);
            var listResult = new List<ModelCommonSelect2>();
            foreach (var item in data)
            {
                listResult.Add(new ModelCommonSelect2()
                {
                    id = item.SubjectCode,
                    text = item.SubjectName
                });
            }
            return Ok(listResult);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(SubjectModel subject)
        {
            var data = _subjectBL.Add(subject);
            return Ok(new { data.result, modal = ModalModify(data) });
        }

        public IActionResult Update(string code)
        {
            var subject = _subjectBL.GetByCode(code);
            return View(subject);
        }

        [HttpPost]
        public IActionResult Update(SubjectModel subject)
        {
            var data = _subjectBL.Update(subject);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }

        [HttpPost]
        public IActionResult Delete(string code)
        {
            var data = _subjectBL.Delete(code);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }
    }
}
