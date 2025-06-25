
namespace SubjectRegister.Common
{
    public class ModelSearchCommon
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ModelCommonSelect2
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    /// <summary>
    /// Model dùng chung để lấy kết quả truy vấn và thông báo message
    /// </summary>
    public class QueryResultModel
    {
        public bool result { get; set; } = true;
        public string? message { get; set; }
    }

    /// <summary>
    /// Model dùng chung để lấy data và phân trang
    /// </summary>
    /// <typeparam name="T">T là model được định nghĩa khi khởi tạo</typeparam>
    public class GetDataPaginationModel<T> where T : class
    {
        public List<T> ListData { get; set; }
        public int TotalPage { get; set; }
        public int PageActive { get; set; }
    }

    /// <summary>
    /// Model dùng chung cho modal thông báo cơ bản
    /// </summary>
    public class ModalModifyModel
    {
        public string? ModalId { get; set; }
        public string? ModalTitle { get; set; }
        public string ModalContent { get; set; }
    }

    public class DepartmentAndSubjectModel
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }

    public class Subject
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string DepartmentCode { get; set; }
        public string PlannedCode { get; set; }
    }

    public class DashboardCounts
    {
        public int TotalStudent { get; set; }
        public int TotalTeacher { get; set; }
        public int TotalClassroom { get; set; }
    }
}