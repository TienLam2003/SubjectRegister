using System.ComponentModel.DataAnnotations;

namespace SubjectRegister.Models
{
    public class StudentModel
    {
        public int StudentCode { get; set; }
        public string Fullname { get; set; }
        public string Class { get; set; }
        public DateTime YearOfBirth { get; set; }
        /// <summary>
        /// Giới tính : 1 là nam, 0 là nữ
        /// </summary>
        public bool Gender { get; set; }
        public string GenderStr => Gender ? "Nam" : "Nữ";
        //public string GenderStr1
        //{
        //    get
        //    {
        //        var genderStr = "";
        //        if (Gender)
        //        {
        //            genderStr = "Nam";
        //        }
        //        else
        //        {
        //            genderStr = "Nữ";
        //        }
        //        return genderStr;
        //        //return Gender ? "Nam" : "Nữ";
        //    }
        //}
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }
        public string Email { get; set; }
        public IFormFile ImageFile { get; set; }
        public string? ImagePath { get; set; }
        public string? DepartmentCode { get; set; } //Khoa
        public int TotalCredits { get; set; } //Tổng số tín chỉ
        public int UserCode { get; set; }
    }

    public class StudentSearchModel
    {
        public string StudentCode { get; set; }
        public string Fullname { get; set; }
        public int PageActive { get; set; } = 1;
    }

    /// <summary>
    /// Nhận file Excel từ submit Form
    /// </summary>
    public class ReadFileExcel
    {
        [Required(ErrorMessage = "Vui lòng chọn file Excel.")]
        public IFormFile ExcelFile { get; set; }
    }
}