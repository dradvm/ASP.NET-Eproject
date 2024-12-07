using ABCDMall.Filters;
using ABCDMall.Models;
using System.Linq;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.feedback = db.Feedbacks.OrderByDescending(item => item.SendingTime).ToList();
            return View();
        }

        public ActionResult SendFeedback()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
