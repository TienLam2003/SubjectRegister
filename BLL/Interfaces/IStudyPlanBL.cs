using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface IStudyPlanBL : ICommon<StudyPlanModel>
    {
        List<SubjectModel> GetSubjectAsPlanedSubject(string departmentCode);
        List<StudyPlanModel> GetByStudent(string studentCode);
        StudyPlanModel GetByCode(string code);
        QueryResultModel Delete(int code);
    }
}
