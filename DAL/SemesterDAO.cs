using Dapper;
using Microsoft.Data.SqlClient;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.DAL
{
    public class SemesterDAO : CommonDAO<SemesterModel>
    {
        public SemesterDAO(IDbConnection dbConnection) : base(dbConnection) { }

        public List<SemesterModel> GetAll()
        {
            var sql = "SELECT * FROM Semester ORDER BY SemesterCode DESC";
            var data = _dbConnection.Query<SemesterModel>(sql).ToList();
            return data;
        }

        /// <summary>
        /// Lấy học kỳ có trạng thái là true(đang hoạt động)
        /// </summary>
        /// <returns></returns>
        public List<SemesterModel> GetByStatus()
        {
            var sql = "SELECT SemesterCode, SemesterName FROM Semester WHERE StatusActive = 1";
            var data = _dbConnection.Query<SemesterModel>(sql).ToList();
            return data;
        }

        public SemesterModel GetByCode(int code)
        {
            string sql = @"SELECT * FROM Semester WHERE SemesterCode = @SemesterCode";
            var data = _dbConnection.QueryFirstOrDefault<SemesterModel>(sql, new { SemesterCode = code });
            return data;
        }

        public QueryResultModel Add(SemesterModel semester)
        {
            var sql = @"INSERT INTO Semester (SemesterName, StartTime, EndTime)
                                    VALUES (@SemesterName, @StartTime, @EndTime)";

            return base.Add(sql, semester, SqlErrorHandler.GetErrorMessage);
        }

        public QueryResultModel Update(SemesterModel semester)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = @"UPDATE Semester
                            SET
                                SemesterName = @SemesterName,
                                StartTime = @StartTime,
                                EndTime = @EndTime
                            WHERE
                                SemesterCode = @SemesterCode";

                _dbConnection.Execute(sql, new
                {
                    semester.SemesterName,
                    semester.StartTime,
                    semester.EndTime,
                    semester.SemesterCode
                });
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel UpdateStatus(int code, bool status)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = @"UPDATE Semester
                            SET
                                StatusActive = @StatusActive
                            WHERE
                                SemesterCode = @SemesterCode";

                _dbConnection.Execute(sql, new { StatusActive = status, SemesterCode = code });
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel Delete(int code)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = "DELETE FROM Semester WHERE SemesterCode = @SemesterCode";
                _dbConnection.Execute(sql, new { SemesterCode = code });
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }
    }
}
