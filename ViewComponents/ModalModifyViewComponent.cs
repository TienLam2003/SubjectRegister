using Microsoft.AspNetCore.Mvc;
using SubjectRegister.Common;

namespace SubjectRegister.ViewComponents
{
    public class ModalModifyViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ModalModifyModel modal)
        {
            return View(modal);
        }
    }
}
