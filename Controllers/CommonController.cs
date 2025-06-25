using Microsoft.AspNetCore.Mvc;
using SubjectRegister.Common;
using SubjectRegister.Common.Extensions;

namespace SubjectRegister.Controllers
{
    public class CommonController : Controller
    {
        public string ModalModify(QueryResultModel resultModel)
        {
            //render partial view với dạng string
            return this.RenderPartialViewToString("_ModelNotifyPartial", resultModel);
        }
    }
}
