using SubjectRegister.BLL.Interfaces;

namespace SubjectRegister.Common.HostedService
{
    /// <summary>
    /// Lớp này được gọi khi chạy hệ thống
    /// </summary>
    public class StartupService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public StartupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                //sử dụng DI để lấy một service có lifetime là Scoped trong một service có lifetime là Singleton
                var planSchedule = scope.ServiceProvider.GetRequiredService<ISetTimePlanSchedule>();
                var registerClassSection = scope.ServiceProvider.GetRequiredService<ISetTimeRegisterClassSection>();

                await planSchedule.SetTimePlanSchedule();
                await registerClassSection.SetTimeRegisterClassSection();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
