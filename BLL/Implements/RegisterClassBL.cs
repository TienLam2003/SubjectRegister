using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.DAL;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Implements
{
    public class RegisterClassBL : IRegisterClassBL
    {
        private readonly RegisterClassDAO _registerClassDAO;
        public RegisterClassBL(RegisterClassDAO registerClassDAO) 
        {
            _registerClassDAO = registerClassDAO;
        }

        /// <summary>
        /// Lấy ds lớp học theo đăng ký của sinh viên
        /// </summary>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        public List<RegisterClassModel> GetClassByStudent(string studentCode)
        {
            return _registerClassDAO.GetClassByStudent(studentCode);
        }

        /// <summary>
        /// Lấy lịch học dựa trên các lớp mà sinh viên đăng ký
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public List<RegisterClassModel> GetScheduleByStudent(string subjectCode)
        {
            return _registerClassDAO.GetScheduleByStudent(subjectCode);
        }

        public QueryResultModel AddRegister(int classCode, string studentCode)
        {
            var data = _registerClassDAO.AddRegister(classCode, studentCode);
            if (data.result)
                data.message = "Đăng ký thành công";
            return data;
        }

        public QueryResultModel Delete(int registerCode)
        {
            return _registerClassDAO.Delete(registerCode);
        }
    }
}
