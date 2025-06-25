using Dapper;
using Microsoft.Data.SqlClient;
using SubjectRegister.BLL.Implements;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.Models;
using SubjectRegister.ViewModels;
using System.Data;
using System.Text;

namespace SubjectRegister.DAL
{
    public class ClassSectionDAO : CommonDAO<ClassSectionModel>
    {
        public ClassSectionDAO(IDbConnection dbConnection) : base(dbConnection) { }

        public async Task<List<ClassSectionModel>> Get(int? semesterCode, string? deparmentCode)
        {
            object param = new { SemesterCode = semesterCode, DeparmentCode = deparmentCode };
            string sql = "EXEC sp_GetStudySchedule;";
            var data = await _dbConnection.QueryAsync<ClassSectionModel>(sql);
            // .ConfigureAwait(false) tránh 1 số lỗi ngữ cảnh
            return data.ToList();
        }

        public List<StudentListPdfViewModel> StudentListByClass(int classCode)
        {
            string sql = @"EXEC GetClassDetails @ClassCode = @ClassCode";
            var result = _dbConnection.Query<StudentListPdfViewModel>(sql, new { ClassCode = classCode }).ToList();
            return result;
        }

        /// <summary>
        /// Hàm lấy tất cả môn học trong bảng kế hoạch
        /// </summary>
        /// <param name="semesterCode"></param>
        /// <returns></returns>
        public List<DepartmentAndSubjectModel> GetDepartmentsWithSubjects(int semesterCode)
        {
            var sql = $@"SELECT d.DepartmentCode,
                        d.DepartmentName,
                        p.PlannedCode,
                        s.SubjectCode,
                        s.SubjectName,
                        s.DepartmentCode AS SubjectDepartmentCode
                        FROM Subject s
                        LEFT JOIN PlannedSubject ps ON s.SubjectCode = ps.SubjectCode AND ps.SemesterCode = {semesterCode}
                        JOIN Department d ON d.DepartmentCode = s.DepartmentCode
                        RIGHT JOIN PlannedSubject p ON p.SubjectCode = s.SubjectCode";

            var departmentDict = new Dictionary<string, DepartmentAndSubjectModel>();

            var result = _dbConnection.Query<DepartmentAndSubjectModel, Subject, DepartmentAndSubjectModel>(
                sql,
                (department, subject) =>
                {
                    // Nếu khoa chưa tồn tại trong dictionary, thêm mới
                    if (!departmentDict.TryGetValue(department.DepartmentCode, out var existingDepartment))
                    {
                        existingDepartment = department;
                        departmentDict[department.DepartmentCode] = existingDepartment;
                    }

                    // Nếu môn học không null, thêm vào danh sách môn học của khoa
                    if (subject != null)
                    {
                        existingDepartment.Subjects.Add(subject);
                    }

                    return existingDepartment;
                },
                splitOn: "SubjectCode" // Chỉ định cột bắt đầu ánh xạ sang Subject
            );

            return departmentDict.Values.ToList();
        }

        public List<TeacherModel> GetTeacherBySubject(string subjectCode)
        {
            string sql = @"SELECT 
                                t.TeacherCode, 
                                t.Fullname,
                                d.DepartmentName
                            FROM 
                                Teacher t
                            JOIN 
                                Department d ON t.DepartmentCode = d.DepartmentCode
                            JOIN 
                                Subject s ON d.DepartmentCode = s.DepartmentCode
                            WHERE 
                                s.SubjectCode = @SubjectCode";
            var data = _dbConnection.Query<TeacherModel>(sql, new { SubjectCode = subjectCode }).ToList();
            return data;
        }

        public List<ClassroomModel> GetAllClassroom()
        {
            string sql = "SELECT * FROM Classroom";
            var data = _dbConnection.Query<ClassroomModel>(sql).ToList();
            return data;
        }

        public QueryResultModel Add(ClassSectionModel model)
        {
            var queryResult = new QueryResultModel();
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    // Thêm bản ghi vào bảng ClassSection
                    string insertClassSectionQuery = @"
                    INSERT INTO ClassSection (PlannedCode, TeacherCode, ClassroomCode, MaxQuantity)
                    VALUES (@PlannedCode, @TeacherCode, @ClassroomCode, @MaxQuantity);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    int classCode = _dbConnection.ExecuteScalar<int>(insertClassSectionQuery, model, transaction);

                    // Thêm các bản ghi vào bảng ClassSchedule
                    string insertClassScheduleQuery = @"
                    INSERT INTO ClassSchedule (ClassCode, ScheduleStudy)
                    VALUES (@ClassCode, @ScheduleStudy);";

                    var classSchedules = model.ListSchedule.Select(day => new
                    {
                        ClassCode = classCode,
                        ScheduleStudy = day
                    });

                    _dbConnection.Execute(insertClassScheduleQuery, classSchedules, transaction);
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    queryResult.message = SqlErrorHandler.GetErrorMessageByClassSection(ex);
                    queryResult.result = false;
                }
                return queryResult;
            }
        }

        public QueryResultModel Update(ClassSectionModel model)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = @"UPDATE PlannedSubject SET 
                                PlanningStart = @PlanningStart,
                                PlanningDeadline = @PlanningDeadline
                            WHERE 
                                PlannedCode = @PlannedCode";
                _dbConnection.Execute(sql, model);
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
            var sql = "DELETE FROM ClassSection WHERE ClassCode = @ClassCode";
            return base.Delete(sql, new { ClassCode = code });
        }

        /// <summary>
        /// Lấy mã kế hoạch theo mã học kỳ và mã môn học
        /// </summary>
        /// <returns></returns>
        public int GetPlannedCodeBySubject(int semesterCode, string subjectCode)
        {
            string sql = "SELECT PlannedCode FROM PlannedSubject WHERE SemesterCode = @SemesterCode AND SubjectCode = @SubjectCode";
            return _dbConnection.QueryFirstOrDefault<int>(sql, new { SemesterCode = semesterCode, SubjectCode = subjectCode });
        }
    }
}
