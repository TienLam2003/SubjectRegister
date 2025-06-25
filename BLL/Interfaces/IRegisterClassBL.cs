using SubjectRegister.Common;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface IRegisterClassBL
    {
        /// <summary>
        /// Lấy lớp học phần tương ứng với sinh viên
        /// </summary>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        List<RegisterClassModel> GetClassByStudent(string studentCode);
        /// <summary>
        /// Lấy các lớp học phần theo môn học đã lập kế hoạch trước đó
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        List<RegisterClassModel> GetScheduleByStudent(string studentCode);
        QueryResultModel AddRegister(int classCode, string studentCode);
        QueryResultModel Delete(int registerCode);
    }
}
