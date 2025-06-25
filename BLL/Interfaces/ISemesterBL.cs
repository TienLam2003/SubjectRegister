using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface ISemesterBL : ICommon<SemesterModel>
    {
        /// <summary>
        /// Lấy học kỳ theo trạng thái true(đang hoạt động)
        /// </summary>
        /// <returns></returns>
        List<SemesterModel> GetByStatus();
        QueryResultModel UpdateStatus(int code, bool status);
        List<SemesterModel> GetAll();
        SemesterModel GetByCode(int code);
        QueryResultModel Delete(int code);
    }
}
