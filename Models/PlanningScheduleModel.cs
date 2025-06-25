namespace SubjectRegister.Models
{
    /// <summary>
    /// Lịch trình kế hoạch
    /// </summary>
    public class PlanScheduleModel
    {
        public int ScheduleId { get; set; }
        public DateTime? PlanStart { get; set; }
        public DateTime? PlanDeadline { get; set; }
    }

    /// <summary>
    /// Lịch trình đăng ký 
    /// </summary>
    public class RegisterScheduleModel
    {
        public int RegisterId { get; set; }
        public DateTime? RegisterStart { get; set; }
        public DateTime? RegisterDeadline { get; set; }
    }

    /// <summary>
    /// Lưu trữ tĩnh khoảng thời gian lập kế hoạch
    /// </summary>
    public static class PlanScheduleModelStatic
    {
        public static DateTime? PlanStart { get; set; }
        public static DateTime? PlanDeadline { get; set; }
    }

    /// <summary>
    /// Lưu trữ tĩnh khoảng thời gian đăng ký
    /// </summary>
    public static class RegisterScheduleModelStatic
    {
        public static DateTime? RegisterStart { get; set; }
        public static DateTime? RegisterDeadline { get; set; }
    }
}
