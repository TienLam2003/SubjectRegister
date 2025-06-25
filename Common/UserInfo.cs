using System.Web;

namespace SubjectRegister.Common
{
    public class UserInfo
    {
        /// <summary>
        /// Mã của tài khoản đăng nhập
        /// </summary>
        public int UserCode { get; set; }
        /// <summary>
        /// Mã sinh viên nếu người dùng là sinhvien ngược lại là mã giảng viên
        /// </summary>
        public string Code { get; set; }
        public string Fullname { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string DepartmentCode { get; set; }
        public string ImagePath { get; set; }
    }
}