using Dapper;
using Microsoft.Data.SqlClient;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.DAL
{
    public class TeacherDAO : CommonDAO<TeacherModel>
    {
        public TeacherDAO(IDbConnection dbConnection) : base(dbConnection) { }

        public GetDataPaginationModel<TeacherModel> GetPagination(int pageActive)
        {
            // Lưu ý : đọc kết quả truy vấn theo thứ tự
            string sql = @"SELECT COUNT(1) FROM Teacher;
                           SELECT * FROM Teacher
                           ORDER BY TeacherCode DESC
                           OFFSET @Offset ROWS 
                           FETCH NEXT @PageSize ROWS ONLY";
            var data = base.GetPagination(pageActive, sql);
            return data;
        }

        public TeacherModel GetByCode(string code)
        {
            string sql = @"SELECT * FROM Teacher WHERE TeacherCode = @TeacherCode";
            var data = _dbConnection.QueryFirstOrDefault<TeacherModel>(sql, new { TeacherCode = code });
            return data;
        }

        public GetDataPaginationModel<TeacherModel> GetBySearch(TeacherSearchModel modelSearch)
        {
            string sql = @"SELECT COUNT(1) FROM Teacher;
                           SELECT * FROM Teacher
                           WHERE (@TeacherCode IS NULL OR TeacherCode = @TeacherCode)
                           AND (@Fullname IS NULL OR Fullname LIKE '%' + @Fullname + '%')
                           ORDER BY TeacherCode DESC
                           OFFSET @Offset ROWS 
                           FETCH NEXT @PageSize ROWS ONLY";
            var data = base.GetBySearch(modelSearch.PageActive, sql, modelSearch);
            return data;
        }

        public QueryResultModel Add(TeacherModel teacher)
        {
            var queryResult = new QueryResultModel();
            _dbConnection.Open();
            var transaction = _dbConnection.BeginTransaction();
            try
            {
                object paramUser = new
                {
                    Username = teacher.TeacherCode,
                    Password = "giangvien",
                    RoleCode = "teacher"
                };
                //Thêm User trước
                var sqlAddUser = @"INSERT INTO Users (Username, Password, RoleCode) VALUES (@Username, @Password, @RoleCode); 
                                    SELECT CAST(SCOPE_IDENTITY() as int)";
                teacher.UserCode = _dbConnection.QuerySingle<int>(sqlAddUser, paramUser, transaction);

                //Sau khi thêm User thêm Giảng viên
                var sqlAddTeacher = @"INSERT INTO Teacher (TeacherCode, Fullname, YearOfBirth, Gender, Phone, ImagePath, DepartmentCode, Degree, UserCode)
                                    VALUES (@TeacherCode, @Fullname, @YearOfBirth, @Gender, @Phone, @ImagePath, @DepartmentCode, @Degree ,@UserCode)";
                _dbConnection.Execute(sqlAddTeacher, new
                {
                    teacher.TeacherCode,
                    teacher.Fullname,
                    teacher.YearOfBirth,
                    teacher.Gender,
                    teacher.Phone,
                    teacher.ImagePath,
                    teacher.DepartmentCode,
                    teacher.Degree,
                    teacher.UserCode
                }, transaction);

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

        public QueryResultModel Update(TeacherModel teacher)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = @"UPDATE Teacher
                            SET
                                Fullname = @Fullname,
                                YearOfBirth = @YearOfBirth,
                                Gender = @Gender,
                                Phone = @Phone,
                                DepartmentCode = @DepartmentCode
                            WHERE
                                TeacherCode = @TeacherCode";

                _dbConnection.Execute(sql, new
                {
                    teacher.Fullname,
                    teacher.YearOfBirth,
                    teacher.Gender,
                    teacher.Phone,
                    teacher.ImagePath,
                    teacher.DepartmentCode,
                    teacher.TeacherCode
                });

                if (!string.IsNullOrEmpty(teacher.ImagePath))
                {
                    sql = @"UPDATE Teacher SET ImagePath = @ImagePath  
                                WHERE TeacherCode = @TeacherCode";
                    _dbConnection.Execute(sql, new
                    {
                        teacher.ImagePath,
                        teacher.TeacherCode
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

        public QueryResultModel Delete(string code)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = "DELETE FROM Teacher WHERE TeacherCode = @TeacherCode";
                _dbConnection.Execute(sql, new { TeacherCode = code });
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
            var sql = "SELECT ImagePath FROM Teacher WHERE TeacherCode = @TeacherCode";
            string imagePath = _dbConnection.QuerySingleOrDefault<string>(sql, new { TeacherCode = code });
            return imagePath;
        }
    }
}
