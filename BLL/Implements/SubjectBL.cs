using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.DAL;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Implements
{
    public class SubjectBL : ISubjectBL
    {
        private readonly SubjectDAO _subjectDAO;
        public SubjectBL(SubjectDAO subjectDAO)
        {
            _subjectDAO = subjectDAO;
        }

        public GetDataPaginationModel<SubjectModel> GetPagination(int pageActive)
        {
            var data = _subjectDAO.GetPagination(pageActive);
            return data;
        }

        public GetDataPaginationModel<SubjectModel> GetBySearch(SubjectSearchModel modelSearch)
        {
            var data = _subjectDAO.GetBySearch(modelSearch);
            return data;
        }

        public SubjectModel GetByCode(string code)
        {
            return _subjectDAO.GetByCode(code);
        }

        public List<SubjectModel> GetSubjectByDepartment(string departmentCode)
        {
            return _subjectDAO.GetSubjectByDepartment(departmentCode);
        }

        public QueryResultModel Add(SubjectModel subject)
        {
            var data = _subjectDAO.Add(subject);
            if (data.result)
                data.message = "Thêm thành công";
            return data;
        }

        public QueryResultModel Update(SubjectModel subject)
        {
            var data = _subjectDAO.Update(subject);
            return data;
        }

        public QueryResultModel Delete(string code)
        {
            var data = _subjectDAO.Delete(code);
            return data;
        }
    }
}
