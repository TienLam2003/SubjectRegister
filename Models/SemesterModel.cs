namespace SubjectRegister.Models
{
    public class SemesterModel
    {
        public int SemesterCode { get; set; }
        public string SemesterName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool StatusActive { get; set; }
        public string strStatusActive => StatusActive ? "Đang hoạt động" : "Không hoạt động";
    }
}
