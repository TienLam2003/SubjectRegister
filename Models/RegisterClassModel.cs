namespace SubjectRegister.Models
{
    public class RegisterClassModel
    {
        public int? ClassCode { get; set; }
        public int? RegisterCode { get; set; }
        public string StudySchedule { get; set; } //List lịch học đã được xử lý khi truy vấn
        public int? DayOfTheWeek { get; set; }
        public int? MaxQuantity { get; set; }
        public int? RegisteredCount { get; set; }
        public string? SemesterName { get; set; }
        public string SubjectName { get; set; }
        public string? TeacherName { get; set; }
        public string? ClassroomName { get; set; }
    }
}
