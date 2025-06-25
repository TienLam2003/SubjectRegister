using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly IDbConnection _dbConnection;
        private readonly IMemoryCache _cache;

        public NotificationViewComponent(IDbConnection dbConnection, IMemoryCache cache)
        {
            _dbConnection = dbConnection;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string studentCode)
        {
            string cacheKey = $"Notification_{studentCode}";

            if (!_cache.TryGetValue(cacheKey, out List<NotificationModel> notifications))
            {
                var sql = @"SELECT TOP 5 * FROM NotificationMessages 
                        WHERE StudentCode = @StudentCode 
                        ORDER BY MessageId DESC";

                var result = await _dbConnection.QueryAsync<NotificationModel>(sql, new { StudentCode = studentCode });
                notifications = result.ToList();

                // Lưu vào cache trong 5 phút
                _cache.Set(cacheKey, notifications, TimeSpan.FromMinutes(5));
            }

            return View(notifications);
        }
    }
}
