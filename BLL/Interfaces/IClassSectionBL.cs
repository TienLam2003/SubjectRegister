using SubjectRegister.Common;
using SubjectRegister.Models;
using SubjectRegister.ViewModels;

namespace SubjectRegister.BLL.Interfaces
{
    public interface IClassSectionBL : ICommon<ClassSectionModel>
    {
        /// <summary>
        /// Thêm bất đồng bộ để giải phóng luồng khi truy vấn
        /// </summary>
        /// <param name="semesterCode"></param>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        Task<List<ClassSectionModel>> Get(int? semesterCode, string? departmentCode);
        /// <summary>
        /// Lấy khoa và môn học thuộc khoa trong phần kế hoạch học tập
        /// </summary>
        /// <param name="semesterCode"></param>
        /// <returns></returns>
        List<DepartmentAndSubjectModel> GetDepartmentsWithSubjects(int semesterCode);
        List<StudentListPdfViewModel> StudentListByClass(int classCode);
        QueryResultModel Delete(int code);
        /// <summary>
        /// Lấy giảng viên theo môn học dựa vào khoa
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <returns></returns>
        List<TeacherModel> GetTeacherBySubject(string subjectCode);
        /// <summary>
        /// Lấy tất cả phòng học
        /// </summary>
        /// <returns></returns>
        List<ClassroomModel> GetAllClassroom();
    }
}
