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
    public class ShoppingCentersController : Controller
    {
        //**************************************GET***********************************
        private readonly ABCDMallEntities db = new ABCDMallEntities();

        [AdminFilter]
        public ActionResult Index()
        {
            ViewBag.shops = db.Shops
                              .Include(s => s.ShopType) // Nạp dữ liệu của ShopType
                              .Where(s => s.ShopType.ID == 1)
                              .ToList();
            return View();
        }







        //**************************************ADD***********************************
        [AdminFilter]
        public ActionResult Add(int status = 1)
        {
            //ViewBag.status = status;
            //ViewBag.shops = db.ShopTypes.ToList();
            //return View();
            var shopTypes = db.ShopTypes
                                .Where(s => s.ID == 1)
                                .ToList(); // Lấy danh sách các ShopType từ database
            var shopTypeItems = shopTypes.Select(st => new SelectListItem
            {
                Value = st.ID.ToString(), // Giá trị của SelectListItem là ID của ShopType
                Text = st.Name // Hiển thị tên ShopType trong dropdown
            }).ToList();

            ViewBag.shops = shopTypeItems; // Gán vào ViewBag để truyền sang View
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

            // Kiểm tra tên cửa hàng
            if (string.IsNullOrEmpty(shop.Name))
            {
                return Redirect("/shoppingcenters/add?status=0");
            }

            // Kiểm tra hình ảnh
            if (logo == null || logo.ContentLength == 0)
            {
                return Redirect("/shoppingcenters/add?status=-1");
            }

            // Kiểm tra định dạng hình ảnh
            string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
            if (!supported.Any(format => string.Equals(format, Path.GetExtension(logo.FileName), StringComparison.OrdinalIgnoreCase)))
            {
                return Redirect("/shoppingcenters/add?status=-2");
            }

            // Kiểm tra kích thước hình ảnh
            if (logo.ContentLength > 10000000)
            {
                return Redirect("/shoppingcenters/add?status=-3");
            }

            // Lưu trữ hình ảnh
            shop.Logo = Path.GetFileNameWithoutExtension(logo.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(logo.FileName);
            logo.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images/Shop"), shop.Logo));

            db.Shops.Add(shop);
            db.SaveChanges();

            return Redirect("/shoppingcenters/index");
        }








        //**************************************UPDATE***********************************
        [AdminFilter]
        public ActionResult Update(int id, int status = 1)
        {
            // Lấy thông tin cửa hàng theo ID
            Shop shop = db.Shops.FirstOrDefault(s => s.ID == id);
            if (shop == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách các loại cửa hàng từ database
            var shopTypes = db.ShopTypes.ToList();
            var shopTypeItems = shopTypes.Select(st => new SelectListItem
            {
                Value = st.ID.ToString(),
                Text = st.Name
            }).ToList();

            // Truyền dữ liệu vào ViewBag
            ViewBag.shops = shopTypeItems; // Danh sách các loại cửa hàng
            ViewBag.status = status; // Trạng thái trả về từ POST

            return View(shop); // Truyền đối tượng Shop vào View
        }

        // POST: ShoppingCenters/Update/5
        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, string name, HttpPostedFileBase logo, int shopTypeID, int floor, string description)
        {
            // Lấy cửa hàng cần cập nhật
            Shop shop = db.Shops.FirstOrDefault(s => s.ID == id);
            if (shop == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra và cập nhật các trường dữ liệu
            if (string.IsNullOrEmpty(name?.Trim()))
            {
                return Redirect($"/shoppingcenters/update/{id}?status=0"); // Kiểm tra tên cửa hàng
            }
            shop.Name = name.Trim();

            if (string.IsNullOrEmpty(description?.Trim()))
            {
                return Redirect($"/shoppingcenters/update/{id}?status=-6"); // Kiểm tra mô tả
            }
            shop.Description = description.Trim();

            // Cập nhật ShopType và Floor
            shop.ShopeType = shopTypeID;
            shop.Floor = floor;

            // Kiểm tra và xử lý logo (nếu có)
            if (logo != null && logo.ContentLength > 0)
            {
                string[] supported = { ".png", ".jpg", ".jpeg", ".svg" };
                if (!supported.Any(format => string.Equals(format, Path.GetExtension(logo.FileName), StringComparison.OrdinalIgnoreCase)))
                {
                    return Redirect($"/shoppingcenters/update/{id}?status=-2"); // Kiểm tra định dạng ảnh
                }

                if (logo.ContentLength > 10000000)
                {
                    return Redirect($"/shoppingcenters/update/{id}?status=-3"); // Kiểm tra kích thước ảnh
                }

                // Xóa ảnh cũ (nếu có)
                string oldLogo = shop.Logo;
                if (System.IO.File.Exists(Server.MapPath("~/Assets/Images/Shop/" + oldLogo)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Assets/Images/Shop/" + oldLogo));
                }

                // Lưu trữ logo mới
                shop.Logo = Path.GetFileNameWithoutExtension(logo.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(logo.FileName);
                logo.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images/Shop"), shop.Logo));
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            return Redirect("/shoppingcenters/index"); // Điều hướng về trang danh sách
        }






        //****************************************DELETE***********************************
        [AdminFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var shop = db.Shops.FirstOrDefault(item => item.ID == id);
            if (shop != null)
            {
                // Xóa logo cũ nếu có
                string oldFile = Path.Combine(Server.MapPath("~/Assets/Images/Shop"), shop.Logo);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                // Xóa shop khỏi cơ sở dữ liệu
                db.Shops.Remove(shop);
                db.SaveChanges();

                // Trả về thông báo thành công
                return Json(new { success = true, message = "Delete successful!" });
            }

            // Trả về thông báo lỗi nếu không tìm thấy shop
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