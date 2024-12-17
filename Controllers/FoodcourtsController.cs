using ABCDMall.Filters;
using ABCDMall.Models;
using ABCDMall.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ABCDMall.Controllers
{
    public class FoodcourtsController : Controller
    {
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]

        public ActionResult Index()
        {
            ViewBag.shops = db.Shops.Include(s => s.ShopType).Where(s => s.ShopType.ID == 2).ToList();
            return View();
        }
        [AdminFilter]
        public ActionResult Add(int status = 1)
        {

            var shopTypes = db.ShopTypes.ToList();
            var shopTypeItems = shopTypes.Select(st => new SelectListItem
            {
                Value = st.ID.ToString(),
                Text = st.Name
            }).ToList();

            ViewBag.shops = shopTypeItems;
            return View();
        }

        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(string name, HttpPostedFileBase logo, int shopTypeID, int floor, string description)
        {
            Shop shop = new Shop();
            shop.ShopeType = shopTypeID;
            shop.Floor = floor;
            shop.Name = name?.Trim();
            shop.Description = description?.Trim();

            if (string.IsNullOrEmpty(shop.Name))
            {
                return Redirect("/foodcourts/add?status=0");
            }
            if (logo == null || logo.ContentLength == 0)
            {
                return Redirect("/foodcourts/add?status=-1");
            }
            string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
            if (!supported.Any(format => string.Equals(format, Path.GetExtension(logo.FileName), StringComparison.OrdinalIgnoreCase)))
            {
                return Redirect("/foodcourts/add?status=-2");
            }
            if (logo.ContentLength > 10000000)
            {
                return Redirect("/foodcourts/add?status=-3");
            }
            shop.Logo = Path.GetFileNameWithoutExtension(logo.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(logo.FileName);
            logo.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images/Shop"), shop.Logo));

            db.Shops.Add(shop);
            db.SaveChanges();

            return Redirect("/foodcourts/index");
        }

        [AdminFilter]
        public ActionResult Update(int id, int status = 1)
        {
            Shop shop = db.Shops.FirstOrDefault(s => s.ID == id);
            if (shop == null)
            {
                return HttpNotFound();
            }

            var shopTypes = db.ShopTypes.ToList();
            var shopTypeItems = shopTypes.Select(st => new SelectListItem
            {
                Value = st.ID.ToString(),
                Text = st.Name
            }).ToList();

            ViewBag.shops = shopTypeItems;
            ViewBag.status = status;

            return View(shop);
        }
        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, string name, HttpPostedFileBase logo, int shopTypeID, int floor, string description)
        {
            Shop shop = db.Shops.FirstOrDefault(s => s.ID == id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            if (string.IsNullOrEmpty(name?.Trim()))
            {
                return Redirect($"/foodcourts/update/{id}?status=0");
            }
            shop.Name = name.Trim();

            if (string.IsNullOrEmpty(description?.Trim()))
            {
                return Redirect($"/foodcourts/update/{id}?status=-6");
            }
            shop.Description = description.Trim();

            shop.ShopeType = shopTypeID;
            shop.Floor = floor;

            if (logo != null && logo.ContentLength > 0)
            {
                string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
                if (!supported.Any(format => string.Equals(format, Path.GetExtension(logo.FileName), StringComparison.OrdinalIgnoreCase)))
                {
                    return Redirect($"/foodcourts/update/{id}?status=-2");
                }

                if (logo.ContentLength > 10000000)
                {
                    return Redirect($"/foodcourts/update/{id}?status=-3");
                }

                string oldLogo = shop.Logo;
                if (System.IO.File.Exists(Server.MapPath("~/Assets/Images/Shop/" + oldLogo)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Assets/Images/Shop/" + oldLogo));
                }

                shop.Logo = Path.GetFileNameWithoutExtension(logo.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(logo.FileName);
                logo.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images/Shop"), shop.Logo));
            }

            db.SaveChanges();

            return Redirect("/foodcourts/index");
        }
        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var shop = db.Shops.FirstOrDefault(item => item.ID == id);
            if (shop != null)
            {

                string oldFile = Path.Combine(Server.MapPath("~/Assets/Images/Shop"), shop.Logo);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
                db.Shops.Remove(shop);
                db.SaveChanges();
                return Json(new { success = true, message = "Delete successful!" });
            }
            return Json(new { success = false, message = "Shop not found." });
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