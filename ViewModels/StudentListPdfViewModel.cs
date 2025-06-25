using SubjectRegister.Models;

namespace SubjectRegister.ViewModels
{
    public class StudentListPdfViewModel
    {
        public string StudentCode { get; set; }
        public string Fullname { get; set; }
        public string Class { get; set; }
        public bool Gender { get; set; }
        public string GenderStr => Gender ? "Nam" : "Nữ";
        public DateTime YearOfBirth { get; set; }
        public string Phone { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        public int NumberOfStudent { get; set; }
        public string StudySchedule { get; set; }
    }
}
