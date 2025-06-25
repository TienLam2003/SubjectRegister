using Dapper;
using SubjectRegister.Common;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.DAL
{
    public class LoginDAO
    {
        private readonly IDbConnection _dbConnection;
        public LoginDAO(IDbConnection dbConnection) 
        { 
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Hàm kiểm tra đăng nhập
        /// </summary>
        /// <returns></returns>
        public bool Auth(UserModel user)
        {
            string sql = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";
            int result = _dbConnection.ExecuteScalar<int>(sql, new {user.Username, user.Password});
            return result > 0;
        }

        /// <summary>
        /// Lấy thông tin người dùng với username khi đăng nhập thành công
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserInfo GetUser(string username)
        {
            string sql = "EXEC SP_GetUserInfoByUsername @Username = @Username";
            var userInfo = _dbConnection.QueryFirst<UserInfo>(sql, new {Username = username});
            return userInfo;
        }

        /// <summary>
        /// Lấy thông tin sinh viên dựa vào mã sinh viên
        /// </summary>
        /// <returns></returns>
        public StudentModel? GetStudentByCode(string studentCode)
        {
            string sql = "SELECT * FROM Student WHERE StudentCode = @StudentCode";
            var student = _dbConnection.QueryFirstOrDefault<StudentModel>(sql, new { StudentCode = studentCode });
            return student;
        }

        /// <summary>
        /// Lấy thông tin sinh viên dựa vào mã sinh viên
        /// </summary>
        /// <returns></returns>
        public TeacherModel? GetTeacherByCode(string teacherCode)
        {
            string sql = "SELECT * FROM Teacher WHERE TeacherCode = @TeacherCode";
            var teacher = _dbConnection.QueryFirstOrDefault<TeacherModel>(sql, new { TeacherCode = teacherCode });
            return teacher;
        }
    }
}
