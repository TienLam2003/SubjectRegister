using Dapper;
using Microsoft.Data.SqlClient;
using SubjectRegister.Common;
using SubjectRegister.Models;
using System.Data;
using SubjectRegister.Comon;

namespace SubjectRegister.DAL
{
    public class StudentDAO : CommonDAO<StudentModel>
    {
        public StudentDAO(IDbConnection dbConnection) : base(dbConnection) { }

        public GetDataPaginationModel<StudentModel> GetPagination(int pageActive)
        {
            // Lưu ý : đọc kết quả truy vấn theo thứ tự
            string sql = @"SELECT COUNT(1) FROM Student;
                           SELECT * FROM Student
                           ORDER BY StudentCode DESC
                           OFFSET @Offset ROWS 
                           FETCH NEXT @PageSize ROWS ONLY";
            var data = base.GetPagination(pageActive, sql);
            return data;
        }

        public StudentModel GetByCode(string code)
        {
            string sql = @"SELECT * FROM Student WHERE StudentCode = @StudentCode";
            var data = _dbConnection.QueryFirstOrDefault<StudentModel>(sql, new { StudentCode = code });
            return data;
        }

        public GetDataPaginationModel<StudentModel> GetBySearch(StudentSearchModel modelSearch)
        {
            string sql = @"SELECT COUNT(1) FROM Student;
                           SELECT * FROM Student
                           WHERE (@StudentCode IS NULL OR StudentCode = @StudentCode)
                           AND (@Fullname IS NULL OR Fullname LIKE '%' + @Fullname + '%')
                           ORDER BY StudentCode DESC
                           OFFSET @Offset ROWS 
                           FETCH NEXT @PageSize ROWS ONLY";
            var data = base.GetBySearch(modelSearch.PageActive, sql, modelSearch);
            return data;
        }

        public QueryResultModel Add(StudentModel student)
        {
            var queryResult = new QueryResultModel();
            _dbConnection.Open();
            var transaction = _dbConnection.BeginTransaction();
            try
            {
                object paramUser = new
                {
                    Username = student.StudentCode,
                    Password = "sinhvien",
                    RoleCode = "student"
                };
                //Thêm User trước
                var sqlAddUser = @"INSERT INTO Users (Username, Password, RoleCode) VALUES (@Username, @Password, @RoleCode); 
                                    SELECT CAST(SCOPE_IDENTITY() as int)";
                student.UserCode = _dbConnection.QuerySingle<int>(sqlAddUser, paramUser, transaction);
                
                //Sau khi thêm User thêm Student
                var sqlAddStudent = @"INSERT INTO Student (StudentCode, Fullname, Class, YearOfBirth, Gender, Phone, ImagePath, DepartmentCode, TotalCredits, UserCode)
                                    VALUES (@StudentCode, @Fullname, @Class, @YearOfBirth, @Gender, @Phone, @ImagePath, @DepartmentCode, @TotalCredits, @UserCode)";
                _dbConnection.Execute(sqlAddStudent, student, transaction);

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel Update(StudentModel student)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = @"UPDATE Student
                            SET
                                Fullname = @Fullname,
                                Class = @Class,
                                YearOfBirth = @YearOfBirth,
                                Gender = @Gender,
                                Phone = @Phone,
                                DepartmentCode = @DepartmentCode
                            WHERE
                                StudentCode = @StudentCode";

                _dbConnection.Execute(sql, new
                {
                    student.Fullname,
                    student.Class,
                    student.YearOfBirth,
                    student.Gender,
                    student.Phone,
                    student.ImagePath,
                    student.DepartmentCode,
                    student.StudentCode
                });

                if (!string.IsNullOrEmpty(student.ImagePath)) 
                {
                    sql = @"UPDATE Student SET ImagePath = @ImagePath  
                                WHERE StudentCode = @StudentCode";
                    _dbConnection.Execute(sql, new
                    {
                        student.ImagePath,
                        student.StudentCode
                    });
                }
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel UpdatePassword(string password, int userCode)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = "UPDATE Users SET Password = @Password WHERE UserCode = @UserCode";
                _dbConnection.Execute(sql, new { Password = password, UserCode = userCode });
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
                var sql = "DELETE FROM Student WHERE StudentCode = @StudentCode";
                _dbConnection.Execute(sql, new { StudentCode = code });
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public string GetImagePath(string code)
        {
            var sql = "SELECT ImagePath FROM Student WHERE StudentCode = @StudentCode";
            string imagePath = _dbConnection.QuerySingleOrDefault<string>(sql, new { StudentCode = code });
            return imagePath;
        }
    }
}
