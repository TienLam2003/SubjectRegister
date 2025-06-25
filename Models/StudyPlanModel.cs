namespace SubjectRegister.Models
{
    public class StudyPlanModel
    {
        public int StudyPlanCode { get; set; }
        public string StudentCode { get; set; }
        public string Fullname { get; set; }
        public int PlannedCode { get; set; }
        public int NumberOfCredits { get; set; }
        public string SubjectName { get; set; }
        public string SemesterName { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
