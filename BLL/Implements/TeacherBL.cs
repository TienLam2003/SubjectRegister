using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common;
using SubjectRegister.Comon;
using SubjectRegister.DAL;
using SubjectRegister.Models;

namespace SubjectRegister.BLL.Implements
{
    public class TeacherBL : ITeacherBL
    {
        private readonly TeacherDAO _teacherDAO;
        private readonly FileManagement _fileManagement;
        public TeacherBL(TeacherDAO teacherDAO, FileManagement fileManagement)
        {
            _teacherDAO = teacherDAO;
            _fileManagement = fileManagement;
        }

        public GetDataPaginationModel<TeacherModel> GetPagination(int pageActive)
        {
            var data = _teacherDAO.GetPagination(pageActive);
            return data;
        }

        public GetDataPaginationModel<TeacherModel> GetBySearch(TeacherSearchModel modelSearch)
        {
            var data = _teacherDAO.GetBySearch(modelSearch);
            return data;
        }

        public TeacherModel GetByCode(string code)
        {
            return _teacherDAO.GetByCode(code);
        }

        public QueryResultModel Add(TeacherModel teacher)
        {
            // Lưu ảnh và nhận đường dẫn lưu ĐB (nếu ảnh tồn tại), ngược lại lấy avatar mặc định CData.Mode.AvatarNone
            if (teacher.ImageFile != null && teacher.ImageFile.Length > 0)
                teacher.ImagePath = _fileManagement.ImageSave(teacher.ImageFile);
            else
                teacher.ImagePath = CData.Mode.AvatarNone;

            var data = _teacherDAO.Add(teacher);
            //Nếu thêm thất bại thì xóa ảnh, ngược lại thì ghi nội dung để thông báo
            if (!data.result)
                _fileManagement.ImageDelete(teacher.ImagePath);
            else
                data.message = "Thêm thành công";
            return data;
        }

        public QueryResultModel Update(TeacherModel teacher)
        {
            // Xử lý file ảnh nếu tồn tại
            if (teacher.ImageFile != null && teacher.ImageFile.Length > 0)
                teacher.ImagePath = _fileManagement.ImageSave(teacher.ImageFile);

            var data = _teacherDAO.Update(teacher);
            return data;
        }

        public QueryResultModel Delete(string code)
        {
            //Lấy đường dẫn và xóa ảnh
            string imagePath = _teacherDAO.GetImagePath(code);
            _fileManagement.ImageDelete(imagePath);
            //Xóa dữ liệu
            var data = _teacherDAO.Delete(code);
            return data;
        }
    }
}
