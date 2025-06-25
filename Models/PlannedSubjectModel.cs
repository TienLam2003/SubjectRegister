namespace SubjectRegister.Models
{
    public class PlannedSubjectModel
    {
        public int PlannedCode {  get; set; }
        public string SubjectCode {  get; set; }
        public string SubjectName {  get; set; }
        public int SemesterCode {  get; set; }
        public string SemesterName {  get; set; }
        public string DepartmentName {  get; set; }
        public int TotalStudyPlans {  get; set; }
    }
}
