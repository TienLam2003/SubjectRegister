using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.DAL;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.BLL.Implements
{
    public class PlannedSubjectBL : IPlannedSubjectBL
    {
        private readonly PlannedSubjectDAO _plannedSubjectDAO;
        public PlannedSubjectBL(PlannedSubjectDAO plannedSubjectDAO) 
        {
            _plannedSubjectDAO = plannedSubjectDAO;
        }

        public GetDataPaginationModel<PlannedSubjectModel> GetPagination(int pageActive)
        {
            var data = _plannedSubjectDAO.GetPagination(pageActive);
            return data;
        }

        public PlannedSubjectModel GetByCode(string code)
        {
            return _plannedSubjectDAO.GetByCode(code);
        }

        public PlanScheduleModel GetTimePlan()
        {
            return _plannedSubjectDAO.GetTimePlan();
        }

        public List<DepartmentAndSubjectModel> GetDepartmentsWithSubjects(int semesterCode)
        {
            var data = _plannedSubjectDAO.GetDepartmentsWithSubjects(semesterCode);
            return data;
        }

        public QueryResultModel Add(PlannedSubjectModel planSubject)
        {
            var data = _plannedSubjectDAO.Add(planSubject);
            if (data.result)
                data.message = "Thêm thành công";
            return data;
        }

        public QueryResultModel Update(PlannedSubjectModel planSubject)
        {
            var data = _plannedSubjectDAO.Update(planSubject);
            return data;
        }

        public QueryResultModel UpdateTime(PlanScheduleModel model)
        {
            model.ScheduleId = CData.Mode.ScheduleId;
            var data = _plannedSubjectDAO.UpdateTime(model);
            return data;
        }

        public QueryResultModel Delete(int code)
        {
            var data = _plannedSubjectDAO.Delete(code);
            return data;
        }
    }
}
