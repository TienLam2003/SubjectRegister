using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.DAL;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Implements
{
    public class StudyPlanBL : IStudyPlanBL
    {
        private readonly StudyPlanDAO _studyPlanDAO;
        public StudyPlanBL(StudyPlanDAO studyPlanDAO)
        {
            _studyPlanDAO = studyPlanDAO;
        }

        public List<StudyPlanModel> GetByStudent(string studentCode)
        {
            var data = _studyPlanDAO.GetByStudent(studentCode);
            return data;
        }

        public List<SubjectModel> GetSubjectAsPlanedSubject(string departmentCode)
        {
            return _studyPlanDAO.GetSubjectAsPlanedSubject(departmentCode);
        }

        public StudyPlanModel GetByCode(string code)
        {
            return _studyPlanDAO.GetByCode(code);
        }

        public QueryResultModel Add(StudyPlanModel model)
        {
            var data = _studyPlanDAO.Add(model);
            if (data.result)
                data.message = "Thêm thành công";
            return data;
        }

        public QueryResultModel Update(StudyPlanModel model)
        {
            var data = _studyPlanDAO.Update(model);
            return data;
        }

        public QueryResultModel Delete(int code)
        {
            var data = _studyPlanDAO.Delete(code);
            return data;
        }
    }
}
