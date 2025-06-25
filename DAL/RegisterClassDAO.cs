using Dapper;
using Microsoft.Data.SqlClient;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.DAL
{
    public class RegisterClassDAO : CommonDAO<RegisterClassModel>
    {
        public RegisterClassDAO(IDbConnection dbConnection) : base(dbConnection) { }

        public List<RegisterClassModel> GetClassByStudent(string studentCode)
        {
            string sql = "EXEC SP_GetClassSectionByStudent @StudentCode = @StudentCode";
            var data = _dbConnection.Query<RegisterClassModel>(sql, new { StudentCode = studentCode }).ToList();
            return data;
        }

        public List<RegisterClassModel> GetScheduleByStudent(string studentCode)
        {
            string sql = "EXEC SP_GetScheduleByStudent @StudentCode = @StudentCode";
            var data = _dbConnection.Query<RegisterClassModel>(sql, new { StudentCode = studentCode }).ToList();
            return data;
        }

        public QueryResultModel AddRegister(int classCode, string studentCode)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = "EXEC AddRegisterClass @StudentCode, @ClassCode";
                _dbConnection.Execute(sql, new { StudentCode = studentCode, ClassCode = classCode });
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessageByRegister(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel Delete(int registerCode)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = "DELETE FROM RegisterClass WHERE RegisterCode = @RegisterCode";
                _dbConnection.Execute(sql, new { RegisterCode = registerCode });
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessageByRegister(ex);
                queryResult.result = false;
            }
            return queryResult;
        }
    }
}
