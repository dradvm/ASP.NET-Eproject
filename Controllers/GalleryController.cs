using ABCDMall.Filters;
using ABCDMall.Models;
using System.Linq;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.gallery = db.Galleries.ToList();
            return View();
        }

        [AdminFilter]
        public ActionResult Add(int status = 1)
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