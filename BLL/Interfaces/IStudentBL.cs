using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface IStudentBL : ICommon<StudentModel>
    {
        StudentModel GetByCode(string code);
        GetDataPaginationModel<StudentModel> GetPagination(int pageActive);
        GetDataPaginationModel<StudentModel> GetBySearch(StudentSearchModel modelSearch);
        QueryResultModel Delete(string code);
        QueryResultModel UpdatePassword(string password, int userCode);
    }
}
