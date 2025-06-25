using Dapper;
using Microsoft.Data.SqlClient;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.DAL
{
    public class SubjectDAO : CommonDAO<SubjectModel>
    {
        public SubjectDAO(IDbConnection dbConnection) : base(dbConnection) { }

        public GetDataPaginationModel<SubjectModel> GetPagination(int pageActive)
        {
            string sql = @"SELECT COUNT(1) FROM Subject;
                           EXEC SP_GetPagedSubjects @Offset = @Offset, @PageSize = @PageSize;";
            var data = base.GetPagination(pageActive, sql);
            return data;
        }

        public SubjectModel GetByCode(string code)
        {
            string sql = @"SELECT * FROM Subject WHERE SubjectCode = @SubjectCode";
            var data = _dbConnection.QueryFirstOrDefault<SubjectModel>(sql, new { SubjectCode = code });
            return data;
        }

        public GetDataPaginationModel<SubjectModel> GetBySearch(SubjectSearchModel modelSearch)
        {
            string sql = @"SELECT COUNT(1) FROM Subject;
                           SELECT * FROM Subject
                           WHERE (@SubjectCode IS NULL OR SubjectCode = @SubjectCode)
                           AND (@SubjectName IS NULL OR SubjectName LIKE '%' + @SubjectName + '%')
                           ORDER BY SubjectCode DESC
                           OFFSET @Offset ROWS 
                           FETCH NEXT @PageSize ROWS ONLY";
            var data = base.GetBySearch(modelSearch.PageActive, sql, modelSearch);
            return data;
        }

        public List<SubjectModel> GetSubjectByDepartment(string departmentCode)
        {
            var sql = @"SELECT SubjectCode, SubjectName FROM Subject 
                        WHERE DepartmentCode = @DepartmentCode";
            var data = _dbConnection.Query<SubjectModel>(sql, new { DepartmentCode = departmentCode}).ToList();
            return data;
        }

        public QueryResultModel Add(SubjectModel subject)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = @"INSERT INTO Subject (SubjectCode, SubjectName, NumberOfCredits, SubjectType, DepartmentCode)
                                    VALUES (@SubjectCode, @SubjectName, @NumberOfCredits, @SubjectType, @DepartmentCode)";
                _dbConnection.Execute(sql, new
                {
                    subject.SubjectCode,
                    subject.SubjectName,
                    subject.NumberOfCredits,
                    subject.SubjectType,
                    subject.DepartmentCode
                });
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
                return queryResult;
            }
            try
            {
                var sql = @"INSERT INTO SubjectPrerequisite (SubjectCode, PrerequisiteCode)
                            SELECT @SubjectCode, @PrerequisiteCode
                            WHERE NOT EXISTS (
                                SELECT 1
                                FROM SubjectPrerequisite
                                WHERE SubjectCode = @SubjectCode AND PrerequisiteCode = @PrerequisiteCode);";
                foreach (var prerequisite in subject.ListPrerequisiteCode)
                {
                    _dbConnection.Execute(sql, new { SubjectCode = subject.SubjectCode, PrerequisiteCode = prerequisite });
                }
            }
            catch
            {

            }
            return queryResult;
        }

        public QueryResultModel Update(SubjectModel subject)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = @"UPDATE Subject
                            SET
                                SubjectName = @SubjectName,
                                NumberOfCredits = @NumberOfCredits,
                                SubjectType = @SubjectType,
                                DepartmentCode = @DepartmentCode
                            WHERE
                                SubjectCode = @SubjectCode";

                _dbConnection.Execute(sql, new
                {
                    subject.SubjectName,
                    subject.NumberOfCredits,
                    subject.SubjectType,
                    subject.DepartmentCode,
                    subject.SubjectCode
                });
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel Delete(string code)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = "DELETE FROM Subject WHERE SubjectCode = @SubjectCode";
                _dbConnection.Execute(sql, new { SubjectCode = code });
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
