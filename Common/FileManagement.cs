using SubjectRegister.Common;

namespace SubjectRegister.Comon
{
    /// <summary>
    /// Lớp xử lí các tập tin
    /// </summary>
    public class FileManagement
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileManagement(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Hàm lưu ảnh và trả ra đường dẫn
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        public string ImageSave(IFormFile imageFile)
        {
            // Tạo tên file dựa trên thời gian hiện tại
            string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + Path.GetFileName(imageFile.FileName);
            // Đường dẫn vật lý của thư mục images trong wwwroot
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            // Đường dẫn đầy đủ của file
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Lưu file vào thư mục
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            // Tạo đường dẫn để lưu vào database
            string imagePath = "/images/" + uniqueFileName;

            return imagePath;
        }

        /// <summary>
        /// Hàm xóa ảnh
        /// </summary>
        /// <param name="imagePath">Nhận vào đường dẫn ảnh từ csdl</param>
        public void ImageDelete(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));

                    // Kiểm tra nếu file tồn tại và file không phải là avatar mặc định thì tiến hành xóa
                    if (File.Exists(fullPath) && imagePath != CData.Mode.AvatarNone)
                    {
                        File.Delete(fullPath);
                    }
                }
                catch
                {
                }
            }
        }
    }
}
