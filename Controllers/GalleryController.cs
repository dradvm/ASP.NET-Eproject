using ABCDMall.Filters;
using ABCDMall.Models;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Web;
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
            ViewBag.status = status;
            return View();
        }

        [AdminFilter]
        [HttpPost]
        public ActionResult Add(HttpPostedFileBase image, string description)
        {
            if (image == null || image.ContentLength == 0)
            {
                return Redirect("/gallery/add?status=0");
            }
            if (image.ContentLength > 10000000)
            {
                return Redirect("/gallery/add?status=-1");
            }
            Debug.WriteLine(image.ContentType);
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