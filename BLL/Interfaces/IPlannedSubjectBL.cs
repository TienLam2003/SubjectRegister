using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface IPlannedSubjectBL : ICommon<PlannedSubjectModel>
    {
        GetDataPaginationModel<PlannedSubjectModel> GetPagination(int pageActive);
        PlannedSubjectModel GetByCode(string code);
        List<DepartmentAndSubjectModel> GetDepartmentsWithSubjects(int semesterCode);
        QueryResultModel Delete(int code);
    }
}
