using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface ISetTimePlanSchedule
    {
        Task SetTimePlanSchedule();
        Task UpdateTimePlanSchedule(PlanScheduleModel model);
    }
}
