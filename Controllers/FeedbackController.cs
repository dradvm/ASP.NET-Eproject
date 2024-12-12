using ABCDMall.Filters;
using ABCDMall.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.feedback = db.Feedbacks.ToList();
            return View();
        }

        public ActionResult Send(int status = 1)
        {
            ViewBag.status = status;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(string email, string content)
        {
            if (content == null || content.Trim().Length == 0)
            {
                return Redirect("/feedback/send?status=0");
            }
            Feedback feedback = new Feedback();
            if (email != null && email.Trim().Length > 0)
            {
                if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    return Redirect("/feedback/send?status=-1");
                }
                feedback.Email = email.Trim();
            }
            feedback.Content = content.Trim();
            feedback.SendingTime = DateTime.Now;
            db.Feedbacks.Add(feedback);
            db.SaveChanges();
            return Redirect("/feedback/send?status=2");
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
