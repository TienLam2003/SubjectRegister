using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.DAL;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Implements
{
    public class StudentBL : IStudentBL
    {
        private readonly StudentDAO _studentDAO;
        private readonly FileManagement _fileManagement;
        public StudentBL(StudentDAO studentDAO, FileManagement fileManagement)
        {
            _studentDAO = studentDAO;
            _fileManagement = fileManagement;
        }

        public GetDataPaginationModel<StudentModel> GetPagination(int pageActive)
        {
            var data = _studentDAO.GetPagination(pageActive);
            return data;
        }

        public GetDataPaginationModel<StudentModel> GetBySearch(StudentSearchModel modelSearch)
        {
            var data = _studentDAO.GetBySearch(modelSearch);
            return data;
        }

        public StudentModel GetByCode(string code)
        {
            return _studentDAO.GetByCode(code);
        }

        public QueryResultModel Add(StudentModel student)
        {
            student.TotalCredits = 0;
            // Lưu ảnh và nhận đường dẫn lưu ĐB (nếu ảnh tồn tại), ngược lại lấy avatar mặc định CData.Mode.AvatarNone
            if (student.ImageFile != null && student.ImageFile.Length > 0)
                student.ImagePath = _fileManagement.ImageSave(student.ImageFile);
            else
                student.ImagePath = CData.Mode.AvatarNone;

            var data = _studentDAO.Add(student);
            //Nếu thêm thất bại thì xóa ảnh, ngược lại thì ghi nội dung để thông báo
            if (!data.result)
                _fileManagement.ImageDelete(student.ImagePath);
            else
                data.message = "Thêm thành công";
            return data;
        }

        public QueryResultModel Update(StudentModel student)
        {
            // Xử lý file ảnh nếu tồn tại
            if (student.ImageFile != null && student.ImageFile.Length > 0)
                student.ImagePath = _fileManagement.ImageSave(student.ImageFile);

            var data = _studentDAO.Update(student);
            return data;
        }

        public QueryResultModel UpdatePassword(string password, int userCode)
        {
            var data = _studentDAO.UpdatePassword(password, userCode);
            if (data.result)
                data.message = "Thay đổi mật khẩu thành công";
            return data;
        }

        public QueryResultModel Delete(string code)
        {
            //Lấy đường dẫn và xóa ảnh
            string imagePath = _studentDAO.GetImagePath(code);
            _fileManagement.ImageDelete(imagePath);
            //Xóa dữ liệu
            var data = _studentDAO.Delete(code);
            return data;
        }
    }
}
