using ABCDMall.Models;
using System.Web;
using System.Web.Mvc;

namespace ABCDMall.Filters
{
    public class AdminFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ((Admin) HttpContext.Current.Session["admin"] == null)
            {
                filterContext.Result = new RedirectResult("~/home/index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}