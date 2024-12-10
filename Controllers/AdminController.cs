using ABCDMall.Filters;
using ABCDMall.Models;
using ABCDMall.Services;
using System.Linq;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class AdminController : Controller
    {
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        public ActionResult Login(int status = 1)
        {
            ViewBag.status = status;
            ViewData["selectedNav"] = "Home";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            password = HashService.GetHash(password);
            Admin admin = db.Admins.FirstOrDefault(item => item.Email == email && item.Password == password);
            if (admin == null)
            {
                return Json("Fail");
            }
            Session["admin"] = admin;
            return Json("/admin/index");
        }

        public ActionResult Logout()
        {
            Session.Remove("admin");
            return Redirect("/home/index");
        }

        [AdminFilter]
        public ActionResult Index()
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