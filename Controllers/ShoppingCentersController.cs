using ABCDMall.Filters;
using ABCDMall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class ShoppingCentersController : Controller
    {
        // GET: ShoppingCenters
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.shops = db.Shops.ToList();
            return View();
        }

        public ActionResult Send(int status = 1)
        {
            ViewBag.status = status;
            return View();
        }





        //02 - Add
        [HttpGet]
        public ActionResult Add()
        {
            // Tạo danh sách cho dropdown nếu cần
            ViewBag.shops = new SelectList(db.ShopTypes, "ID", "Name"); // Tùy chỉnh bảng phù hợp
            ViewData["selectedNav"] = "AdminShop";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Shop shop, HttpPostedFileBase Logo)
        {
            if (ModelState.IsValid)
            {
                if (Logo != null)
                {
                    // Lưu file logo
                    string fileName = System.IO.Path.GetFileName(Logo.FileName);
                    string path = Server.MapPath("~/Assets/Images/Shop" + fileName);
                    Logo.SaveAs(path);
                    shop.Logo = "/Shop/" + fileName;
                }

                db.Shops.Add(shop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.shops = new SelectList(db.ShopTypes, "ID", "Name"); // Tải lại dropdown
            return View(shop);
        }



    }
}