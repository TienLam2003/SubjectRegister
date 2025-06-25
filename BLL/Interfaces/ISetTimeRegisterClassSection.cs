using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface ISetTimeRegisterClassSection
    {
        Task SetTimeRegisterClassSection();
        Task UpdateTimeRegisterClassSection(RegisterScheduleModel model);
    }
}
