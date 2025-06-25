using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.Common;
using SubjectRegister.Models;
using System.Data;
using System.Diagnostics;

namespace SubjectRegister.Controllers
{
    [Authorize] // Yêu cầu xác thực cho toàn bộ controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDbConnection _dbConnection;

        public HomeController(ILogger<HomeController> logger, IDbConnection dbConnection)
        {
            _logger = logger;
            _dbConnection = dbConnection;
        }

        public async Task<IActionResult> Index()
        {
            var totals = await _dbConnection.QueryFirstOrDefaultAsync<DashboardCounts>(@"
                            SELECT 
                            (SELECT COUNT(1) FROM Student) AS TotalStudent,
                            (SELECT COUNT(1) FROM Teacher) AS TotalTeacher,
                            (SELECT COUNT(1) FROM Classroom) AS TotalClassroom");

            if (totals != null)
            {
                ViewBag.TotalStudent = totals.TotalStudent;
                ViewBag.TotalTeacher = totals.TotalTeacher;
                ViewBag.TotalClassroom = totals.TotalClassroom;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}