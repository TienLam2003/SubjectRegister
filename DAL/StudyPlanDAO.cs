using Dapper;
using Microsoft.Data.SqlClient;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.DAL
{
    public class StudyPlanDAO : CommonDAO<StudyPlanModel>
    {
        public StudyPlanDAO(IDbConnection dbConnection) : base(dbConnection) { }

        public List<StudyPlanModel> GetByStudent(string studentCode)
        {
            string sql = @"SELECT sp.StudyPlanCode, sj.SubjectName, sj.NumberOfCredits, sm.SemesterName, sp.RegisterDate
                            FROM StudyPlan sp
                            JOIN PlannedSubject ps ON ps.PlannedCode = sp.PlannedCode
                            JOIN Subject sj ON sj.SubjectCode = ps.SubjectCode
                            JOIN Semester sm ON sm.SemesterCode = ps.SemesterCode AND sm.StatusActive = 1
                            WHERE sp.StudentCode = @StudentCode";
            var data = _dbConnection.Query<StudyPlanModel>(sql, new {StudentCode = studentCode}).ToList();
            return data;
        }

        public List<SubjectModel> GetSubjectAsPlanedSubject(string departmentCode)
        {
            string sql = @"SELECT s.SubjectCode, s.SubjectName, s.NumberOfCredits, p.PlannedCode FROM PlannedSubject p
                            JOIN Subject s ON s.SubjectCode = p.SubjectCode
                            JOIN Semester sm ON sm.SemesterCode = p.SemesterCode AND sm.StatusActive = 1
                            WHERE s.DepartmentCode = @DepartmentCode";
            var data = _dbConnection.Query<SubjectModel>(sql, new { DepartmentCode = departmentCode }).ToList();
            return data;
        }

        public StudyPlanModel GetByCode(string code)
        {
            string sql = @"SELECT * FROM PlannedSubject WHERE PlannedCode = @PlannedCode";
            var data = _dbConnection.QueryFirstOrDefault<StudyPlanModel>(sql, new { PlannedCode = code });
            return data;
        }

        public List<DepartmentAndSubjectModel> GetDepartmentsWithSubjects(int semesterCode)
        {
            var sql = $@"SELECT d.DepartmentCode,
                        d.DepartmentName,
                        s.SubjectCode,
                        s.SubjectName,
                        s.DepartmentCode AS SubjectDepartmentCode
                        FROM Subject s
                        LEFT JOIN PlannedSubject ps ON s.SubjectCode = ps.SubjectCode AND ps.SemesterCode = {semesterCode}
                        JOIN Department d ON d.DepartmentCode = s.DepartmentCode
                        WHERE ps.SubjectCode IS NULL;";

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

        public QueryResultModel Add(StudyPlanModel model)
        {
            var queryResult = new QueryResultModel();
            try
            {
                // Lấy tổng số tín chỉ đã đăng ký của sinh viên
                string sqlCredits = @"
            SELECT ISNULL(SUM(s.NumberOfCredits), 0)
            FROM StudyPlan sp
            JOIN PlannedSubject ps ON sp.PlannedCode = ps.PlannedCode
            JOIN Subject s ON ps.SubjectCode = s.SubjectCode
            WHERE sp.StudentCode = @StudentCode";

                int currentCredits = _dbConnection.QueryFirstOrDefault<int>(sqlCredits, new { model.StudentCode });

                // Lấy số tín chỉ của môn học mới
                string sqlNewCredits = @"
            SELECT s.NumberOfCredits
            FROM PlannedSubject ps
            JOIN Subject s ON ps.SubjectCode = s.SubjectCode
            WHERE ps.PlannedCode = @PlannedCode";

                int newCredits = _dbConnection.QueryFirstOrDefault<int>(sqlNewCredits, new { model.PlannedCode });

                if (currentCredits + newCredits > 25)
                {
                    queryResult.result = false;
                    queryResult.message = "Tổng số tín chỉ đăng ký không được vượt quá 25.";
                    return queryResult;
                }

                var sql = @"INSERT INTO StudyPlan (StudentCode, PlannedCode) 
                    VALUES (@StudentCode, @PlannedCode)";
                _dbConnection.Execute(sql, model);
            }
            catch (SqlException ex)
            {
                queryResult.message = "Mỗi sinh viên không được đăng ký quá 25 chỉ cho 1 học kỳ";
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel Update(StudyPlanModel model)
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
            var queryResult = new QueryResultModel();
            try
            {
                var sql = "DELETE FROM PlannedSubject WHERE PlannedCode = @PlannedCode";
                _dbConnection.Execute(sql, new { PlannedCode = code });
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
