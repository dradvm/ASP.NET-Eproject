using ABCDMall.Filters;
using ABCDMall.Models;
using ABCDMall.Services;
using System;
using System.IO;
using System.Linq;
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
        [ValidateAntiForgeryToken]
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
            string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
            if (!supported.Any(format => string.Equals(format, Path.GetExtension(image.FileName), System.StringComparison.OrdinalIgnoreCase)))
            {
                return Redirect("/gallery/add?status=-2");
            }
            string newName = HashService.GetHash(image.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(image.FileName);
            Gallery gallery = new Gallery();
            gallery.Image = newName;
            if (description != null && description.Trim().Length > 0)
            {
                gallery.Description = description;
            }
            image.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images/Gallery"), newName));
            db.Galleries.Add(gallery);
            db.SaveChanges();
            return Redirect("/gallery/index");
        }

        [AdminFilter]
        public ActionResult Update(int id, int status = 1)
        {
            Gallery gallery = db.Galleries.FirstOrDefault(item => item.ID == id);
            if (gallery == null)
            {
                return Redirect("/gallery/index");
            }
            ViewBag.gallery = gallery;
            ViewBag.status = status;
            return View();
        }

        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, HttpPostedFileBase image, string description)
        {
            Gallery gallery = db.Galleries.FirstOrDefault(item => item.ID == id);
            if (gallery == null)
            {
                return Redirect("/gallery/index");
            }
            if (image != null && image.ContentLength > 0)
            {
                if (image.ContentLength > 10000000)
                {
                    return Redirect("/gallery/update?id=" + id + "&status=0");
                }
                string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
                if (!supported.Any(format => string.Equals(format, Path.GetExtension(image.FileName), System.StringComparison.OrdinalIgnoreCase)))
                {
                    return Redirect("/gallery/update?id=" + id + "&status=-1");
                }
                string newName = HashService.GetHash(image.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(image.FileName);
                string oldFile = Path.Combine(Server.MapPath("~/Assets/Images/Gallery"), gallery.Image);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
                gallery.Image = newName;
                image.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images/Gallery"), newName));
            }
            if (description != null && description.Trim().Length > 0)
            {
                gallery.Description = description;
            }
            else
            {
                gallery.Description = null;
            }
            db.SaveChanges();
            return Redirect("/gallery/index");
        }

        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Gallery gallery = db.Galleries.FirstOrDefault(item => item.ID == id);
            if (gallery != null)
            {
                string oldFile = Path.Combine(Server.MapPath("~/Assets/Images/Gallery"), gallery.Image);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
                db.Galleries.Remove(gallery);
                db.SaveChanges();
            }
            return Json("OK");
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