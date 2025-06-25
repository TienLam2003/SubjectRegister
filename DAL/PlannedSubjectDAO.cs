using Dapper;
using Microsoft.Data.SqlClient;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.Models;
using System.Data;

namespace SubjectRegister.DAL
{
    public class PlannedSubjectDAO : CommonDAO<PlannedSubjectModel>
    {
        public PlannedSubjectDAO(IDbConnection dbConnection) : base(dbConnection) { }

        public GetDataPaginationModel<PlannedSubjectModel> GetPagination(int pageActive)
        {
            // Lưu ý : đọc kết quả truy vấn theo thứ tự
            string sql = @"SELECT COUNT(1) FROM PlannedSubject;
                           SELECT 
                                ps.PlannedCode,
                                ps.SubjectCode,
                                s.SubjectName,
                                d.DepartmentName,
                                d.DepartmentCode,
                                ps.SemesterCode,
                                sem.SemesterName,
	                            COUNT(sp.StudyPlanCode) AS TotalStudyPlans
                            FROM PlannedSubject ps
                            JOIN Subject s ON ps.SubjectCode = s.SubjectCode
                            JOIN Semester sem ON ps.SemesterCode = sem.SemesterCode AND sem.StatusActive = 1
                            JOIN Department d ON d.DepartmentCode = s.DepartmentCode
                            LEFT JOIN StudyPlan sp ON ps.PlannedCode = sp.PlannedCode
                            GROUP BY ps.PlannedCode, ps.SubjectCode, s.SubjectName, 
		                              d.DepartmentName, d.DepartmentCode, ps.SemesterCode, sem.SemesterName
                            ORDER BY ps.PlannedCode DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT @PageSize ROWS ONLY";
            var data = base.GetPagination(pageActive, sql);
            return data;
        }

        public PlannedSubjectModel GetByCode(string code)
        {
            string sql = @"SELECT * FROM PlannedSubject WHERE PlannedCode = @PlannedCode";
            var data = _dbConnection.QueryFirstOrDefault<PlannedSubjectModel>(sql, new { PlannedCode = code });
            return data;
        }

        public PlanScheduleModel GetTimePlan()
        {
            var sql = "SELECT PlanningStart, PlanningDeadline FROM PlanningSchedule";
            return _dbConnection.QuerySingle<PlanScheduleModel>(sql);
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

        public QueryResultModel Add(PlannedSubjectModel planSubject)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = @"INSERT INTO PlannedSubject (SubjectCode, SemesterCode) 
                            VALUES (@SubjectCode, @SemesterCode)";
                _dbConnection.Execute(sql, planSubject);
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel UpdateTime(PlanScheduleModel model)
        {
            var queryResult = new QueryResultModel();
            try
            {
                var sql = @"UPDATE PlanningSchedule SET 
                                PlanningStart = @PlanningStart,
                                PlanningDeadline = @PlanningDeadline
                            WHERE 
                                ScheduleID = @ScheduleID";
                _dbConnection.Execute(sql, model);
            }
            catch (SqlException ex)
            {
                queryResult.message = SqlErrorHandler.GetErrorMessage(ex);
                queryResult.result = false;
            }
            return queryResult;
        }

        public QueryResultModel Update(PlannedSubjectModel model)
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
