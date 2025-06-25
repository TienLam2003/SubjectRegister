namespace SubjectRegister.Models
{
    public class SubjectModel
    {
        public string SubjectCode {  get; set; }
        public string SubjectName {  get; set; }
        public int NumberOfCredits {  get; set; }
        public bool SubjectType {  get; set; }
        public string SubjectTypeStr => SubjectType ? "Bắt buộc" : "Tùy chọn";
        public List<string> ListPrerequisiteCode { get; set; } // Danh sách mã môn học tiên quyết
        public string PrerequisiteCodes { get; set; } // Mã môn học tiên quyết load lên từ csdl
        public string DepartmentCode {  get; set; }
        public string DepartmentName {  get; set; }
        public int PlannedCode {  get; set; }
    }

    public class SubjectSearchModel
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int PageActive { get; set; } = 1;
    }
}
