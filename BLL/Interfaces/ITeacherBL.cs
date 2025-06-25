using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface ITeacherBL : ICommon<TeacherModel>
    {
        TeacherModel GetByCode(string code);
        GetDataPaginationModel<TeacherModel> GetPagination(int pageActive);
        GetDataPaginationModel<TeacherModel> GetBySearch(TeacherSearchModel modelSearch);
        QueryResultModel Delete(string code);
    }
}
