using ABCDMall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCDMall.Controllers
{
    public class HomeController : Controller
    {
        //Khởi tạo DBContext
        private ABCDMallEntities db = new ABCDMallEntities();

        public ActionResult Index()
        {
            return View();
        }

        //Copy đoạn này để tự Dispose DBContext sau khi action hoàn thành, tránh tràn dữ liệu
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