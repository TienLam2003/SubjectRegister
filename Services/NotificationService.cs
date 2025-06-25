using Azure.Messaging;
using Dapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.Services
{
    public class NotificationService : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _cache;
        public NotificationService(IHubContext<NotificationHub> hubContext, IServiceProvider serviceProvider, IMemoryCache cache)
        {
            _hubContext = hubContext;
            _serviceProvider = serviceProvider;
            _cache = cache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                await CheckStudentPlans();
            }
        }

        private async Task CheckStudentPlans()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

            var sql = @"SELECT s.StudentCode, s.FullName
                        FROM Student s
                        WHERE NOT EXISTS (
                            SELECT 1
                            FROM StudyPlan sp
                            JOIN PlannedSubject ps ON sp.PlannedCode = ps.PlannedCode
                            JOIN Semester sem ON ps.SemesterCode = sem.SemesterCode
                            WHERE sp.StudentCode = s.StudentCode
                            AND sem.StatusActive = 1)";

            var students = await dbConnection.QueryAsync<StudentModel>(sql);
            string messageContent = "Thời hạn lập kế hoạch học tập sắp hết!";

            foreach (var student in students)
            {
                // Gửi thông báo qua SignalR tới đúng user (nếu đang online)
                await hubContext.Clients.User(student.StudentCode.ToString())
                    .SendAsync("ReceiveNotification", messageContent);

                // Xóa cache nếu có
                string cacheKey = $"Notification_{student.StudentCode}";
                _cache.Remove(cacheKey);
            }

            var notifications = students.Select(s => new
            {
                MessageContent = messageContent,
                StudentCode = s.StudentCode,
            }).ToList();

            // Thêm hàng loạt vào cơ sở dữ liệu
            var insertSql = @"INSERT INTO NotificationMessages (MessageContent, StudentCode)
                               VALUES (@MessageContent, @StudentCode)";

            await dbConnection.ExecuteAsync(insertSql, notifications);
        }
    }
}
