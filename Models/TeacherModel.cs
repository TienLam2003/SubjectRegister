namespace SubjectRegister.Models
{
    public class TeacherModel
    {
        public string TeacherCode { get; set; }
        public string Fullname { get; set; }
        public DateTime YearOfBirth { get; set; }
        public bool Gender { get; set; }
        public string GenderStr => Gender ? "Nam" : "Nữ";
        public string Phone { get; set; }
        public string Degree { get; set; }
        public IFormFile ImageFile { get; set; }
        public string? ImagePath { get; set; }
        public string DepartmentCode { get; set; }
        public int UserCode { get; set; }
    }

    public class TeacherSearchModel
    {
        public string TeacherCode { get; set; }
        public string Fullname { get; set; }
        public int PageActive { get; set; } = 1;
    }
}