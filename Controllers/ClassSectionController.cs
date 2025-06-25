using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.DAL;
using SubjectRegister.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Rotativa.AspNetCore;
using SubjectRegister.BLL.Implements;
using Microsoft.AspNetCore.Authorization;

namespace SubjectRegister.Controllers
{
    [Authorize(Roles = "admin")]
    public class ClassSectionController : CommonController
    {
        private readonly IClassSectionBL _classSectionBL;
        private readonly ISetTimeRegisterClassSection _setTimeRegister;
        private readonly ISemesterBL _semesterBL;
        public ClassSectionController(IClassSectionBL classSectionBL, ISetTimeRegisterClassSection setTimeRegister, ISemesterBL semesterBL)
        {
            _classSectionBL = classSectionBL;
            _setTimeRegister = setTimeRegister;
            _semesterBL = semesterBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetClassSection(int? semesterCode, string? departmentCode)
        {
            var data = await _classSectionBL.Get(semesterCode, departmentCode);
            return PartialView("_Table", data);
        }

        public IActionResult GetDepartmentsWithSubjects(int semesterCode)
        {
            var data = _classSectionBL.GetDepartmentsWithSubjects(semesterCode);
            // Chuyển đổi dữ liệu thành cấu trúc Select2
            var select2Data = data.Select(department => new
            {
                text = department.DepartmentName, // Tiêu đề nhóm (Khoa)
                children = department.Subjects.Select(subject => new
                {
                    id = subject.SubjectCode, // Giá trị của mã kế hoạch thông qua môn học
                    text = subject.SubjectName // Tên hiển thị của môn học
                }).ToList()
            });
            return Json(select2Data);
        }

        public IActionResult GetTeacherBySubject(string subjectCode)
        {
            var data = _classSectionBL.GetTeacherBySubject(subjectCode);
            //item là tên biến đại diện cho mỗi phần tử trong tập hợp ban đầu
            //=> biểu thị "biểu thức trả về", nghĩa là phần bên phải sẽ được thực thi và trả về cho mỗi item
            var select2Data = data.Select(item => new ModelCommonSelect2
            {
                id = item.TeacherCode,
                text = item.Fullname 
            }).ToList();
            return Json(select2Data);
        }

        public IActionResult Add()
        {
            ViewBag.Semester = _semesterBL.GetByStatus(); //Lấy trạng thái học kỳ đang hoạt động
            ViewBag.Classroom = _classSectionBL.GetAllClassroom(); //Lấy all phòng học
            // Có thể dùng Task.WhenAll để bất đồng bộ và chờ tất cả hoàn thành
            //await Task.WhenAll(ViewBag.Semester, ViewBag.Classroom);
            return View();
        }

        [HttpPost]
        public IActionResult Add(ClassSectionModel model)
        {
            var data = _classSectionBL.Add(model);
            return Ok(new { data.result, modal = ModalModify(data) });
        }

        public IActionResult StudentListByClass(int classCode)
        {
            var data = _classSectionBL.StudentListByClass(classCode);
            ViewBag.ClassCode = classCode;
            return View(data);
        }

        [HttpPost]
        public IActionResult Update(ClassSectionModel model)
        {
            var data = _classSectionBL.Update(model);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTime(RegisterScheduleModel model)
        {
            if (model.RegisterStart <= model.RegisterDeadline)
            {
                await _setTimeRegister.UpdateTimeRegisterClassSection(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int code)
        {
            var data = _classSectionBL.Delete(code);
            if (data.result)
                return Ok(data);
            return PartialView("_ModelNotifyPartial", data);
        }

        public IActionResult ExportPdf(int classCode)
        {
            // Lấy dữ liệu từ cơ sở dữ liệu
            var data = _classSectionBL.StudentListByClass(classCode);

            // Truyền model vào View
            var pdf = new ViewAsPdf("PdfView", data)
            {
                FileName = "Student.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--footer-center \"[page] / [toPage]\" --footer-line --footer-spacing 6"
            };

            return pdf;
        }

        /// <summary>
        /// Xuất dữ liệu ra file Excel
        /// </summary>
        /// <returns>File Excel</returns>
        public IActionResult ExportFile(int classCode)
        {
            // Cài đặt LicenseContext
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Lấy dữ liệu từ cơ sở dữ liệu
            var students = _classSectionBL.StudentListByClass(classCode);

            // Lấy các dữ liệu cần thiết
            var data = students.FirstOrDefault();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Tiêu đề chính
                worksheet.Cells[1, 1, 1, 6].Merge = true; // Gộp các ô từ cột 1 đến cột 6
                worksheet.Cells[1, 1].Value = "Danh sách sinh viên";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 20;
                worksheet.Cells[1, 1].Style.Font.Name = "Times New Roman";
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Thông tin lớp học
                worksheet.Cells[2, 1, 2, 6].Merge = true;
                worksheet.Cells[3, 1, 3, 6].Merge = true;
                worksheet.Cells[4, 1, 4, 6].Merge = true;
                worksheet.Cells[5, 1, 5, 6].Merge = true;
                worksheet.Cells[2, 1].Value = $"Giảng viên giảng dạy: {data.TeacherName}";
                worksheet.Cells[3, 1].Value = $"Môn học: {data.SubjectName}";
                worksheet.Cells[4, 1].Value = $"Lịch học trong tuần: {data.StudySchedule}";
                worksheet.Cells[5, 1].Value = $"Số lượng sinh viên: {students?.Count ?? 0}";

                // Thêm tiêu đề cột
                worksheet.Cells[6, 1].Value = "Mã sinh viên";
                worksheet.Cells[6, 2].Value = "Họ tên";
                worksheet.Cells[6, 3].Value = "Giới tính";
                worksheet.Cells[6, 4].Value = "Năm sinh";
                worksheet.Cells[6, 5].Value = "Lớp";
                worksheet.Cells[6, 6].Value = "Số điện thoại";

                // Định dạng tiêu đề cột
                var headerRange = worksheet.Cells[6, 1, 6, 6];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Font.Color.SetColor(System.Drawing.Color.White);
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Điền dữ liệu vào bảng
                if (students != null && students.Count > 0)
                {
                    for (int i = 0; i < students.Count; i++)
                    {
                        worksheet.Cells[i + 7, 1].Value = students[i].StudentCode;
                        worksheet.Cells[i + 7, 2].Value = students[i].Fullname;
                        worksheet.Cells[i + 7, 3].Value = students[i].GenderStr;
                        worksheet.Cells[i + 7, 4].Value = students[i].YearOfBirth.ToString("dd/MM/yyyy");
                        worksheet.Cells[i + 7, 5].Value = students[i].Class;
                        worksheet.Cells[i + 7, 6].Value = students[i].Phone;
                    }

                    // Định dạng dữ liệu
                    var dataRange = worksheet.Cells[2, 1, students.Count + 6, 6];
                    dataRange.Style.Font.Name = "Times New Roman";
                    dataRange.Style.Font.Size = 15;
                    dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                // Tự động căn chỉnh cột
                worksheet.Cells[1, 1, students.Count + 6, 6].AutoFitColumns();

                // Lưu Excel vào MemoryStream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Trả file Excel
                string fileName = "StudentData.xlsx";
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
