using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SubjectRegister.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Sử dụng StudentCode làm UserId
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
