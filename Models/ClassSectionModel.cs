namespace SubjectRegister.Models
{
    public class ClassSectionModel
    {
        public int ClassCode { get; set; } //Mã lớp học
        public int PlannedCode { get; set; } //Mã kế hoạch
        public int SemesterCode { get; set; } //Mã học kỳ
        public string SubjectCode { get; set; } //Mã môn học
        public string SubjectName { get; set; } //Tên môn học
        public string TeacherCode { get; set; } //Mã giảng viên
        public string TeacherName { get; set; } //Tên giảng viên
        public string ClassroomCode { get; set; } //Mã phòng học
        public string StudySchedule { get; set; } //List lịch học đã được xử lý khi truy vấn
        public int MaxQuantity { get;set; } //Số lượng tối đa
        public int RegisteredCount { get;set; } //Số lượng sinh viên đã đăng ký
        public List<int> ListSchedule { get; set;} //List lịch học gửi từ client
    }
}
