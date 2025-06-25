using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface ISubjectBL : ICommon<SubjectModel>
    {
        SubjectModel GetByCode(string code);
        GetDataPaginationModel<SubjectModel> GetPagination(int pageActive);
        GetDataPaginationModel<SubjectModel> GetBySearch(SubjectSearchModel modelSearch);
        List<SubjectModel> GetSubjectByDepartment(string departmentCode);
        QueryResultModel Delete(string code);
    }
}
