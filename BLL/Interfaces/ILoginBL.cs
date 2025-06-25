using SubjectRegister.Models;

namespace SubjectRegister.BLL.Interfaces
{
    public interface ILoginBL
    {
        Task<bool> Auth(UserModel user);
        StudentModel GetStudentByCode(string code);
        TeacherModel GetTeacherByCode(string code);
    }
}
