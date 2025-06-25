using Dapper;
using Microsoft.AspNetCore.Mvc;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.ViewComponents
{
    public class DepartmentViewComponent : ViewComponent
    {
        private readonly IDbConnection _dbConnection;
        public DepartmentViewComponent(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Component in ra combobox danh sách khoa, nhận vào mã khoa để xác định khoa selected
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(string departmentCode = "")
        {
            var sql = "SELECT * FROM Department";
            ViewBag.Data = _dbConnection.Query<DepartmentModel>(sql).ToList();
            ViewBag.DepartmentCode = departmentCode;
            return View();
        }
    }
}
