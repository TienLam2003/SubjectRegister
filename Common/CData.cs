namespace SubjectRegister.Common
{
    public class CData
    {
        public struct Mode
        {
            /// <summary>
            /// Giá trị số dòng trên 1 trang 
            /// </summary>
            public const int PageSize = 10;
            /// <summary>
            /// Đường dẫn mặc định của ảnh khi là null
            /// </summary>
            public const string AvatarNone = "/images/AvatarNone.jfif";
            /// <summary>
            /// ID thời gian mở cho sinh viên lập kế hoạch học tập
            /// </summary>
            public const int ScheduleId = 1;
            /// <summary>
            /// ID thời gian mở cho sinh viên đăng ký lớp học
            /// </summary>
            public const int RegisterId = 1;
        }
    }
}
