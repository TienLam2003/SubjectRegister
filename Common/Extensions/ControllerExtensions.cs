using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SubjectRegister.Common.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Hàm mở rộng cho controller
        /// </summary>
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            // Lấy ViewEngine từ dịch vụ DI
            var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

            // Gán model cho ViewData
            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                // Tìm View
                var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new FileNotFoundException($"Không tìm thấy View: {viewName}");
                }

                // Tạo ViewContext
                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                // Render View
                viewResult.View.RenderAsync(viewContext);
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
