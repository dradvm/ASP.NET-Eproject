using ABCDMall.Filters;
using ABCDMall.Models;
using System.Linq;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class MovieController : Controller
    {
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.movies = db.Movies.ToList();
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