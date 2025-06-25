using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.DAL;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Implements
{
    public class SemesterBL : ISemesterBL
    {
        public readonly SemesterDAO _semesterDAO;
        public SemesterBL(SemesterDAO semesterDAO)
        {
            _semesterDAO = semesterDAO;
        }

        public List<SemesterModel> GetByStatus()
        {
            var data = _semesterDAO.GetByStatus();
            return data;
        }

        public List<SemesterModel> GetAll()
        {
            var data = _semesterDAO.GetAll();
            return data;
        }

        public SemesterModel GetByCode(int code)
        {
            return _semesterDAO.GetByCode(code);
        }

        public QueryResultModel Add(SemesterModel semester)
        {
            var data = _semesterDAO.Add(semester);
            if (data.result)
                data.message = "Thêm thành công";
            return data;
        }

        public QueryResultModel Update(SemesterModel semester)
        {
            var data = _semesterDAO.Update(semester);
            return data;
        }

        public QueryResultModel UpdateStatus(int code, bool status)
        {
            status = !status; //Đảo ngược giá trị trạng thái hoạt động
            var data = _semesterDAO.UpdateStatus(code, status);
            return data;
        }

        public QueryResultModel Delete(int code)
        {
            var data = _semesterDAO.Delete(code);
            return data;
        }
    }
}
