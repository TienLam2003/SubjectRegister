using Microsoft.Data.SqlClient;

namespace SubjectRegister.Comon
{
    public static class SqlErrorHandler
    {
        /// <summary>
        /// Hàm xử lý lỗi SQL và trả về thông báo chung
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetErrorMessage(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627: // Lỗi trùng mã
                case 2601: // Lỗi trùng giá trị unique key
                    return "Mã đã tồn tại";
                case 547:  // Lỗi vi phạm ràng buộc khóa ngoại
                    return "Không thể xóa do dữ liệu liên quan đang được sử dụng";
                case 515:  // Lỗi bỏ qua trường bắt buộc (not null constraint)
                    return "Thiếu thông tin bắt buộc, vui lòng kiểm tra và nhập đầy đủ dữ liệu";
                case 2628: // Độ dài chuỗi vượt quá giới hạn cho phép
                    return "Chuỗi dữ liệu trong trường có độ dài vượt quá qui định";
                case 245:  // Lỗi chuyển đổi kiểu dữ liệu
                    return "Lỗi chuyển đổi kiểu dữ liệu. Vui lòng kiểm tra lại dữ liệu đầu vào.";
                default:
                    return $"Lỗi không xác định";
            }
        }

        /// <summary>
        /// Hàm thông báo lỗi vi phạm khi sinh viên thêm môn cho kế hoạch
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetErrorMessageByStudyPlan(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627: // Lỗi trùng mã
                case 2601: // Lỗi trùng giá trị unique key
                    return "Bạn đã thêm môn học này rồi!";
                case 547:  // Lỗi vi phạm ràng buộc khóa ngoại
                    return "Không tìm thấy mã kế hoạch vừa nhập!";
                default:
                    return $"Đã có lỗi xảy ra hãy thử lại sau";
            }
        }

        /// <summary>
        /// Hàm thông báo lỗi vi phạm khi sinh viên đăng ký lớp học
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetErrorMessageByRegister(SqlException ex)
        {
            if (ex.Message.Contains("UQ_Student_Planned"))
                return "Bạn đã đăng ký học môn này ở lớp khác rồi";
            else
                return "Đã xảy ra lỗi không xác định";
        }

        /// <summary>
        /// Thông báo lỗi cụ thể khi thêm lớp học mới
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetErrorMessageByClassSection(SqlException ex)
        {
            if (ex.Message.Contains("FK_ClassSection_PlannedSubject"))
                return "Mã kế hoạch không tồn tại";
            else if (ex.Message.Contains("FK_ClassSection_Teacher"))
                return "Mã giảng viên không tồn tại";
            else if (ex.Message.Contains("FK_ClassSection_Classroom"))
                return "Mã phòng học không tồn tại";
            else if (ex.Number == 50001)
                return "Phòng học đã được sử dụng cho ngày và buổi học này";
            else if (ex.Number == 50002)
                return "Giảng viên đã có lịch dạy cho ngày và buổi học này";
            else
                return "Đã xảy ra lỗi không xác định khi thêm dữ liệu";
        }
    }
}
