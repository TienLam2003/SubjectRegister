using Dapper;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.BLL.Implements
{
    public class SetTime : ISetTimePlanSchedule, ISetTimeRegisterClassSection
    {
        private readonly IDbConnection _dbConnection;
        public SetTime(IDbConnection dbConnection) => _dbConnection = dbConnection;

        /// <summary>
        /// Hàm set thời gian lập kế hoạch vào biễn tĩnh
        /// </summary>
        /// <returns></returns>
        public async Task SetTimePlanSchedule()
        {
            string sql = "SELECT PlanStart, PlanDeadline FROM PlanSchedule WHERE ScheduleId = @ScheduleId";
            var data = await _dbConnection.QueryFirstOrDefaultAsync<PlanScheduleModel>(sql, new { ScheduleId = CData.Mode.ScheduleId });
            PlanScheduleModelStatic.PlanStart = data.PlanStart;
            PlanScheduleModelStatic.PlanDeadline = data.PlanDeadline;
        }

        /// <summary>
        /// Hàm sửa thời gian lập kế hoạch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateTimePlanSchedule(PlanScheduleModel model)
        {
            var sql = @"UPDATE PlanSchedule SET 
                                PlanStart = @PlanStart,
                                PlanDeadline = @PlanDeadline
                            WHERE 
                                ScheduleId = @ScheduleId";
            model.ScheduleId = CData.Mode.ScheduleId;
            _dbConnection.Execute(sql, model);
            //Gọi lại hàm Set để cập nhật dữ liệu
            await SetTimePlanSchedule();
        }

        /// <summary>
        /// Hàm set thời gian mở đăng ký vào các biến tĩnh
        /// </summary>
        /// <returns></returns>
        public async Task SetTimeRegisterClassSection()
        {
            string sql = "SELECT RegisterStart, RegisterDeadline FROM RegisterSchedule WHERE RegisterId = @RegisterId";
            var data = _dbConnection.QueryFirstOrDefault<RegisterScheduleModel>(sql, new { RegisterId = CData.Mode.RegisterId });
            RegisterScheduleModelStatic.RegisterStart = data.RegisterStart;
            RegisterScheduleModelStatic.RegisterDeadline = data.RegisterDeadline;
        }

        /// <summary>
        /// Hàm sửa thời gian đăng ký
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateTimeRegisterClassSection(RegisterScheduleModel model)
        {
            var sql = @"UPDATE RegisterSchedule SET 
                                RegisterStart = @RegisterStart,
                                RegisterDeadline = @RegisterDeadline
                            WHERE 
                                RegisterId = @RegisterId";
            model.RegisterId = CData.Mode.RegisterId;
            _dbConnection.Execute(sql, model);
            //Gọi lại hàm Set để cập nhật dữ liệu
            await SetTimeRegisterClassSection();
        }
    }
}
