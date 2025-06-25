using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.DAL;
using SubjectRegister.Models;
using SubjectRegister.ViewModels;

namespace SubjectRegister.BLL.Implements
{
    public class ClassSectionBL : IClassSectionBL
    {
        private readonly ClassSectionDAO _classSectionDAO;
        public ClassSectionBL(ClassSectionDAO classSectionDAO)
        {
            _classSectionDAO = classSectionDAO;
        }

        public List<TeacherModel> GetTeacherBySubject(string subjectCode)
        {
            return _classSectionDAO.GetTeacherBySubject(subjectCode);
        }

        public List<ClassroomModel> GetAllClassroom()
        {
            return _classSectionDAO.GetAllClassroom();
        }

        public async Task<List<ClassSectionModel>> Get(int? semesterCode, string? deparmentCode)
        {
            var data = await _classSectionDAO.Get(semesterCode, deparmentCode);
            return data;
        }

        public List<StudentListPdfViewModel> StudentListByClass(int classCode)
        {
            return _classSectionDAO.StudentListByClass(classCode);
        }

        public List<DepartmentAndSubjectModel> GetDepartmentsWithSubjects(int semesterCode)
        {
            var data = _classSectionDAO.GetDepartmentsWithSubjects(semesterCode);
            return data;
        }

        public QueryResultModel Add(ClassSectionModel model)
        {
            model.PlannedCode = _classSectionDAO.GetPlannedCodeBySubject(model.SemesterCode, model.SubjectCode);
            var data = _classSectionDAO.Add(model);
            if (data.result)
                data.message = "Thêm thành công";
            return data;
        }

        public QueryResultModel Update(ClassSectionModel model)
        {
            var data = _classSectionDAO.Update(model);
            return data;
        }

        public QueryResultModel Delete(int code)
        {
            var data = _classSectionDAO.Delete(code);
            return data;
        }
    }
}
